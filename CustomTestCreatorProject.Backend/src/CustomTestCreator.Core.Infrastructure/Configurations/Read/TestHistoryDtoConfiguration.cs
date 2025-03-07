using CustomTestCreator.SharedKernel.ValueObjects.Dtos;
using CustomTestCreator.SharedKernel.ValueObjects.Dtos.ForQuery;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomTestCreator.Core.Infrastructure.Configurations.Read;

public class TestHistoryDtoConfiguration : IEntityTypeConfiguration<TestHistoryDto>
{
    public void Configure(EntityTypeBuilder<TestHistoryDto> builder)
    {
        builder.ToTable("TestHistories");
        
        builder.HasKey(i => i.Id);
        
        builder.Property(ci => ci.CreatorId);
        builder.Property(ti => ti.TestId);
        
        builder.Property(coaa => coaa.CountOfAllAnswers);
        builder.Property(cora => cora.CountOfRightAnswers);
        
        builder.Property(pt => pt.PassageTime);
    }
}