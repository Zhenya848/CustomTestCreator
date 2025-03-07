using System.Text.Json;
using CustomTestCreator.SharedKernel.ValueObjects.Dtos;
using CustomTestCreator.SharedKernel.ValueObjects.Dtos.ForQuery;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomTestCreator.Core.Infrastructure.Configurations.Read;

public class TestDtoConfiguration : IEntityTypeConfiguration<TestDto>
{
    public void Configure(EntityTypeBuilder<TestDto> builder)
    {
        builder.ToTable("Tests");
        
        builder.HasKey(i => i.Id);
        
        builder.Property(tn => tn.TestName);

        builder.ComplexProperty(lt => lt.LimitTime, ltb =>
        {
            ltb.Property(s => s.Seconds).IsRequired();
            ltb.Property(m => m.Minutes).IsRequired();
            ltb.Property(h => h.Hours).IsRequired();
        });
        
        builder.Property(itl => itl.IsTimeLimited);
        
        builder.Property(vl => vl.Verdicts).HasConversion(
                list => JsonSerializer.Serialize(list, JsonSerializerOptions.Default),
                json => JsonSerializer.Deserialize<IEnumerable<string>>(json, JsonSerializerOptions.Default)!)
            .HasColumnType("jsonb")
            .HasColumnName("verdicts");
    }
}