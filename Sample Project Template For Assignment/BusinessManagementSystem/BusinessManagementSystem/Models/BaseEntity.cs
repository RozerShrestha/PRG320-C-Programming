using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace BusinessManagementSystem.Models
{
    public abstract class BaseEntity
    {
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public class BaseEntityConfiguration : IEntityTypeConfiguration<BaseEntity>
        {
            public void Configure(EntityTypeBuilder<BaseEntity> builder)
            {
                builder.Property(x => x.CreatedBy).HasColumnType("varchar(150)");
                builder.Property(x => x.UpdatedBy).HasColumnType("varchar(150)");

            }
        }
    }
}
