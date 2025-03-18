using CustomTestCreator.SharedKernel.ValueObjects.Id;
using CustomTestCreator.TestHistories.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomTestCreator.TestHistories.Infrastructure.Configurations;

public class TestHistoryConfiguration : IEntityTypeConfiguration<TestHistory>
{
    public void Configure(EntityTypeBuilder<TestHistory> builder)
    {
        builder.ToTable("testHistories");
        
        builder.Property(i => i.Id).HasConversion(id => id.Value, value => TestHistoryId.Create(value));   
        builder.HasKey(i => i.Id);
        
        builder.Property(ci => ci.CreatorId).IsRequired();
        builder.Property(ti => ti.TestId).IsRequired();
        
        builder.Property(coaa => coaa.CountOfAllAnswers).IsRequired();
        builder.Property(cora => cora.CountOfRightAnswers).IsRequired();
        
        builder.Property(pt => pt.PassageTime).IsRequired();
    }
}