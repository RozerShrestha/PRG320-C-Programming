using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessManagementSystem.Models
{
    public class Role:BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<UserRole>? UserRoles { get; set; }
        public virtual ICollection<MenuRole>? MenuRoles { get; set; }
    }
    public class RoleEntityConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.Property(x => x.Name).HasColumnType("varchar(50)");
            builder.Property(x => x.Id).ValueGeneratedNever();
        }
    }
}
