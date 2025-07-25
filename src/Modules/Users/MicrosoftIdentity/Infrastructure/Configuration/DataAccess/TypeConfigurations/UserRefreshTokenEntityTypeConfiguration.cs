using CompanyName.MyMeetings.Modules.UsersMI.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CompanyName.MyMeetings.Modules.UsersMI.Infrastructure.Configuration.DataAccess.TypeConfigurations;

internal class UserRefreshTokenEntityTypeConfiguration : IEntityTypeConfiguration<UserRefreshToken>
{
    public void Configure(EntityTypeBuilder<UserRefreshToken> builder)
    {
        builder.ToTable("UserRefreshTokens", "usersmi").HasKey(k => k.Id);

        builder.Property(p => p.Token).IsRequired();
        builder.Property(p => p.JwtId).IsRequired();
        builder.Property(p => p.IsRevoked).IsRequired();
        builder.Property(p => p.AddedDate).IsRequired();
        builder.Property(p => p.ExpiryDate).IsRequired();

        builder.HasOne(p => p.User).WithMany().IsRequired();
        builder.Navigation(p => p.User).AutoInclude();
    }
}