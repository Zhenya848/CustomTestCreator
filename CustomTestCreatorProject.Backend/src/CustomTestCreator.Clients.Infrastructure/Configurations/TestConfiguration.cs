using System.Text.Json;
using CustomTestCreator.Clients.Domain;
using CustomTestCreator.SharedKernel;
using CustomTestCreator.SharedKernel.ValueObjects.Id;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomTestCreator.Clients.Infrastructure.Configurations;

public class TestConfiguration : IEntityTypeConfiguration<Test>
{
    public void Configure(EntityTypeBuilder<Test> builder)
    {
        builder.ToTable("Tests");

        builder.Property(i => i.Id).HasConversion(i => i.Value, value => TestId.Create(value));
        builder.HasKey(i => i.Id);
        
        builder.Property(tn => tn.TestName).IsRequired().HasMaxLength(Constants.MIN_LENGTH_OF_STRING_PROPERTIES);

        builder.ComplexProperty(lt => lt.LimitTime, ltb =>
        {
            ltb.Property(s => s.Seconds).IsRequired();
            ltb.Property(m => m.Minutes).IsRequired();
            ltb.Property(h => h.Hours).IsRequired();
        });
        
        builder.Property(itl => itl.IsTimeLimited).IsRequired();

        builder.Property(vl => vl.VerdictsList).HasConversion(
                list => JsonSerializer.Serialize(list, JsonSerializerOptions.Default),
                json => JsonSerializer.Deserialize<List<string>>(json, JsonSerializerOptions.Default)!)
            .HasColumnType("jsonb")
            .HasColumnName("verdicts");
        
        builder.HasMany(t => t.Tasks).WithOne().HasForeignKey("test_id").OnDelete(DeleteBehavior.Cascade);
        
        builder.Property(idlt => idlt.IsDeleted);
        builder.Property(dd => dd.DeletionDate);
    }
}