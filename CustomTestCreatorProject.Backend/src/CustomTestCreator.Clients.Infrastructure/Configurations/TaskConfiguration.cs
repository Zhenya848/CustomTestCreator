using CustomTestCreator.SharedKernel;
using CustomTestCreator.SharedKernel.ValueObjects.Id;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Task = CustomTestCreator.Clients.Domain.Task;

namespace CustomTestCreator.Clients.Infrastructure.Configurations;

public class TaskConfiguration : IEntityTypeConfiguration<Task>
{
    public void Configure(EntityTypeBuilder<Task> builder)
    {
        builder.ToTable("tasks");

        builder.Property(i => i.Id).HasConversion(i => i.Value, value => TaskId.Create(value));
        builder.HasKey(i => i.Id);
        
        builder.Property(tn => tn.TaskName).IsRequired().HasMaxLength(Constants.MIN_LENGTH_OF_STRING_PROPERTIES);
        builder.Property(tm => tm.TaskMessage).IsRequired().HasMaxLength(Constants.MAX_LENGTH_OF_STRING_PROPERTIES);
        builder.Property(ra => ra.RightAnswer).IsRequired().HasMaxLength(Constants.MIN_LENGTH_OF_STRING_PROPERTIES);

        builder.Property(imp => imp.ImagePath);
            
        builder.Property(idlt => idlt.IsDeleted);
        builder.Property(dd => dd.DeletionDate);
    }
}