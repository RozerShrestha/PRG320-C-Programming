using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace BusinessManagementSystem.Models
{
    public class MenuRole
    {
        public int RoleId { get; set; }
        public int MenuId { get; set; }
        public Menu Menu { get; set; }
        public Role Role { get; set; }
    }
    public class MenuRoleEntityConfiguration : IEntityTypeConfiguration<MenuRole>
    {
        public void Configure(EntityTypeBuilder<MenuRole> builder)
        {
            builder.HasKey(x => new { x.RoleId, x.MenuId });

            builder.HasOne<Role>(x => x.Role)
                .WithMany(x => x.MenuRoles)
                .HasForeignKey(x => x.RoleId);

            builder.HasOne<Menu>(x => x.Menu)
                .WithMany(x => x.MenuRoles)
                .HasForeignKey(x => x.MenuId);

        }
    }
}
