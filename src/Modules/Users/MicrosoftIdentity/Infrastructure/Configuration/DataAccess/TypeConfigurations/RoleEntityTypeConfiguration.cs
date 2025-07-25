using CompanyName.MyMeetings.Modules.UsersMI.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CompanyName.MyMeetings.Modules.UsersMI.Infrastructure.Configuration.DataAccess.TypeConfigurations;

internal class RoleEntityTypeConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Roles", "usersmi");
    }
}