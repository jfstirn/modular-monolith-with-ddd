using Autofac;
using Autofac.Extensions.DependencyInjection;
using CompanyName.MyMeetings.API.Configuration;
using CompanyName.MyMeetings.API.Configuration.ExecutionContext;
using CompanyName.MyMeetings.API.Configuration.Extensions;
using CompanyName.MyMeetings.API.Configuration.Validation;
using CompanyName.MyMeetings.API.Modules.Administration;
using CompanyName.MyMeetings.API.Modules.Meetings;
using CompanyName.MyMeetings.API.Modules.Payments;
using CompanyName.MyMeetings.API.Modules.Registrations;
using CompanyName.MyMeetings.BuildingBlocks.Application;
using CompanyName.MyMeetings.BuildingBlocks.Domain;
using CompanyName.MyMeetings.BuildingBlocks.Infrastructure.Authorization;
using CompanyName.MyMeetings.BuildingBlocks.Infrastructure.Emails;
using CompanyName.MyMeetings.BuildingBlocks.Infrastructure.ModuleHosting;
using CompanyName.MyMeetings.Modules.Administration.Infrastructure.Configuration;
using CompanyName.MyMeetings.Modules.Meetings.Infrastructure.Configuration;
using CompanyName.MyMeetings.Modules.Payments.Infrastructure.Configuration;
using CompanyName.MyMeetings.Modules.Registrations.Infrastructure.Configuration;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Serilog;
using Serilog.Formatting.Compact;
using ILogger = Serilog.ILogger;

namespace CompanyName.MyMeetings.API
{
    public class Startup
    {
        private const string MeetingsConnectionString = "MeetingsConnectionString";
        private static ILogger _logger;
        private static ILogger _loggerForApi;
        private readonly IConfiguration _configuration;
        private readonly ModuleLoader _moduleLoader = new();

        public Startup(IWebHostEnvironment env)
        {
            ConfigureLogger();

            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json")
                .AddUserSecrets<Startup>()
                .AddEnvironmentVariables("Meetings_")
                .Build();

            _loggerForApi.Information("Connection string:" + _configuration.GetConnectionString(MeetingsConnectionString));

            AuthorizationChecker.CheckAllEndpoints(typeof(Startup).Assembly);
            RegisterModules(_moduleLoader);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var builder = services.AddControllers();
            builder.PartManager.ApplicationParts.Clear();
            builder.PartManager.ApplicationParts.Add(new AssemblyPart(typeof(Startup).Assembly));

            services.AddSwaggerDocumentation(_moduleLoader);

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IExecutionContextAccessor, ExecutionContextAccessor>();

            services.AddProblemDetails(x =>
            {
                x.Map<InvalidCommandException>(ex => new InvalidCommandProblemDetails(ex));
                x.Map<BusinessRuleValidationException>(ex => new BusinessRuleValidationExceptionProblemDetails(ex));
            });

            _moduleLoader.AddHostServices(services, builder.PartManager);
        }

        public void ConfigureContainer(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterModule(new MeetingsAutofacModule());
            containerBuilder.RegisterModule(new AdministrationAutofacModule());
            containerBuilder.RegisterModule(new PaymentsAutofacModule());
            containerBuilder.RegisterModule(new RegistrationsAutofacModule());
            _moduleLoader.RegisterModules(containerBuilder);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            app.UseCors(builder =>
                builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

            InitializeModules(app);

            app.UseMiddleware<CorrelationMiddleware>();

            app.UseSwaggerDocumentation();

            if (env.IsDevelopment())
            {
                app.UseProblemDetails();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            // app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        private static void ConfigureLogger()
        {
            _logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console(
                    outputTemplate:
                    "[{Timestamp:HH:mm:ss} {Level:u3}] [{Module}] [{Context}] {Message:lj}{NewLine}{Exception}")
                .WriteTo.File(new CompactJsonFormatter(), "logs/logs")
                .CreateLogger();

            _loggerForApi = _logger.ForContext("Module", "API");

            _loggerForApi.Information("Logger configured");
        }

        private void RegisterModules(ModuleLoader moduleLoader)
        {
            UserAccessModuleSelector.AddUserAccessModule(moduleLoader, _configuration);
        }

        private void InitializeModules(IApplicationBuilder applicationBuilder)
        {
            var container = applicationBuilder.ApplicationServices.GetAutofacRoot();
            var httpContextAccessor = container.Resolve<IHttpContextAccessor>();
            var executionContextAccessor = new ExecutionContextAccessor(httpContextAccessor);

            var emailsConfiguration = new EmailsConfiguration(_configuration["EmailsConfiguration:FromEmail"]);

            var hostServices = new HostServices(
                _logger,
                _configuration.GetConnectionString(MeetingsConnectionString),
                _configuration["Security:TextEncryptionKey"],
                applicationBuilder,
                executionContextAccessor,
                emailsConfiguration,
                eventsBus: null);
            _moduleLoader.InitializeModules(hostServices);

            MeetingsStartup.Initialize(
                _configuration.GetConnectionString(MeetingsConnectionString),
                executionContextAccessor,
                _logger,
                emailsConfiguration,
                null);

            AdministrationStartup.Initialize(
                _configuration.GetConnectionString(MeetingsConnectionString),
                executionContextAccessor,
                _logger,
                null);

            PaymentsStartup.Initialize(
                _configuration.GetConnectionString(MeetingsConnectionString),
                executionContextAccessor,
                _logger,
                emailsConfiguration,
                null);

            RegistrationsStartup.Initialize(
                _configuration.GetConnectionString(MeetingsConnectionString),
                executionContextAccessor,
                _logger,
                emailsConfiguration,
                _configuration["Security:TextEncryptionKey"],
                null,
                null);
        }
    }
}