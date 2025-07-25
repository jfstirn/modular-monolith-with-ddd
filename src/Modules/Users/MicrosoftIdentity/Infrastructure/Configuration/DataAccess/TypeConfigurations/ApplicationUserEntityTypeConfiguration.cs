using CompanyName.MyMeetings.Modules.UsersMI.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CompanyName.MyMeetings.Modules.UsersMI.Infrastructure.Configuration.DataAccess.TypeConfigurations;

internal class ApplicationUserEntityTypeConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.ToTable("Users", "usersmi");

        builder.Property<string>("Name").HasMaxLength(255);
        builder.Property<string>("FirstName").HasMaxLength(100);
        builder.Property<string>("LastName").HasMaxLength(100);
    }
}
