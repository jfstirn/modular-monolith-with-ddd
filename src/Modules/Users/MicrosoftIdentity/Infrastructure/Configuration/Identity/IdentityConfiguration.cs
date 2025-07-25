using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace CompanyName.MyMeetings.Modules.UsersMI.Infrastructure.Configuration.Identity;

internal static class IdentityConfiguration
{
    public static IServiceCollection ConfigureIdentityService(this IServiceCollection services, UserAccessConfiguration usersConfiguration)
    {
        return services
            .AddHttpContextAccessor()
            .AddJWTBearerAuthentication(usersConfiguration);
    }

    private static IServiceCollection AddJWTBearerAuthentication(this IServiceCollection services, UserAccessConfiguration userConfiguration)
    {
        services.AddScoped(x => userConfiguration);

        services.AddAuthentication()
                .AddJwtBearer(
                    JwtBearerDefaults.AuthenticationScheme,
                    bearerOptions =>
                    {
                        bearerOptions.RequireHttpsMetadata = false;
                        bearerOptions.SaveToken = true;

                        bearerOptions.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = userConfiguration.GetIssuerSigningKey(),

                            ValidIssuer = userConfiguration.GetValidIssuer(),
                            ValidateIssuer = userConfiguration.ShouldValidateIssuer(),
                            ValidAudience = userConfiguration.GetValidAudience(),
                            ValidateAudience = userConfiguration.ShouldValidateAudience(),
                            ValidateLifetime = true,
                            RequireExpirationTime = true,

                            // Clock skew compensates for server time drift.
                            ClockSkew = TimeSpan.FromSeconds(60)
                        };

                        bearerOptions.Events = new JwtBearerEvents
                        {
                            OnChallenge = context =>
                            {
                                 return Task.CompletedTask;
                            },

                            OnTokenValidated = context =>
                            {
                                return Task.CompletedTask;
                            },
                            OnAuthenticationFailed = context =>
                            {
                                if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                                {
                                    context.Response.Headers.Append("Token-Expired", "true");
                                }

                                return Task.CompletedTask;
                            }
                        };
                    })
                .AddCookie(IdentityConstants.ApplicationScheme)
                .AddCookie(IdentityConstants.TwoFactorUserIdScheme);

        return services;
    }
}