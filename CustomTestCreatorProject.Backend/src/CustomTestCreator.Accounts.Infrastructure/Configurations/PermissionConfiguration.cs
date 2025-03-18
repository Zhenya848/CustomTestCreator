using CustomTestCreator.Accounts.Domain.User;
using CustomTestCreator.SharedKernel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomTestCreator.Accounts.Infrastructure.Configurations;

public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable("permissions");
        
        builder.HasIndex(c => c.Code).IsUnique();
        builder.Property(d => d.Description).HasMaxLength(Constants.MIN_LENGTH_OF_STRING_PROPERTIES);
    }
}