using Microsoft.EntityFrameworkCore;
using CustomTestCreator.Clients.Domain;
using CustomTestCreator.SharedKernel;
using CustomTestCreator.SharedKernel.ValueObjects.Id;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomTestCreator.Clients.Infrastructure.Configurations;

public class ClientConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.ToTable("Clients");

        builder.Property(i => i.Id).IsRequired().HasConversion(i => i.Value, value => ClientId.Create(value));
        builder.HasKey(i => i.Id);
        
        builder.Property(n => n.Name).IsRequired().HasMaxLength(Constants.MIN_LENGTH_OF_STRING_PROPERTIES);

        builder.ComplexProperty(ts => ts.TaskSettings, tsb =>
        {
            tsb.Property(irt => irt.IsRandomTasks).IsRequired().HasColumnName("is_random_tasks");
            tsb.Property(iim => iim.IsInfiniteMode).IsRequired().HasColumnName("is_infinite_mode");
        });
        
        builder.HasMany(t => t.Tests).WithOne().HasForeignKey("client_id").OnDelete(DeleteBehavior.Cascade);

        builder.Property(idlt => idlt.IsDeleted);
        builder.Property(dd => dd.DeletionDate);
    }
}