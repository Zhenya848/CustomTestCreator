using CustomTestCreator.SharedKernel.ValueObjects.Dtos;
using CustomTestCreator.SharedKernel.ValueObjects.Dtos.ForQuery;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomTestCreator.Core.Infrastructure.Configurations.Read;

public class TaskDtoConfiguration : IEntityTypeConfiguration<TaskDto>
{
    public void Configure(EntityTypeBuilder<TaskDto> builder)
    {
        builder.ToTable("Tasks");
        
        builder.HasKey(i => i.Id);
        
        builder.Property(tn => tn.TaskName);
        builder.Property(tm => tm.TaskMessage);
        builder.Property(ra => ra.RightAnswer);

        builder.Property(imi => imi.ImageId);
    }
}