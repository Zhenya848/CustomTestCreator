using System.Text.Json;
using CustomTestCreator.Clients.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Task = CustomTestCreator.Clients.Domain.Task;

namespace CustomTestCreator.Clients.Infrastructure.Configurations;

public class TaskOfChoosingAnswerConfiguration : IEntityTypeConfiguration<TaskOfChoosingAnswer>
{
    public void Configure(EntityTypeBuilder<TaskOfChoosingAnswer> builder)
    {
        builder.HasBaseType<Task>();

        builder.Property(al => al.AnswersList).HasConversion(
            value => JsonSerializer.Serialize(value, JsonSerializerOptions.Default),
            json => JsonSerializer.Deserialize<List<string>>(json, JsonSerializerOptions.Default)!)
            .HasColumnType("jsonb")
            .HasColumnName("answers");
    }
}