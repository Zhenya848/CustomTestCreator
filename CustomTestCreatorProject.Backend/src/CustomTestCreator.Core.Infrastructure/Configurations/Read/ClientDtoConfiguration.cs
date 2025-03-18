using CustomTestCreator.SharedKernel.ValueObjects.Dtos;
using CustomTestCreator.SharedKernel.ValueObjects.Dtos.ForQuery;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomTestCreator.Core.Infrastructure.Configurations.Read;

public class ClientDtoConfiguration : IEntityTypeConfiguration<ClientDto>
{
    public void Configure(EntityTypeBuilder<ClientDto> builder)
    {
        builder.ToTable("clients");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(n => n.Name);

        builder.Property(irt => irt.IsRandomTasks);
        builder.Property(iim => iim.IsInfiniteMode);
    }
}