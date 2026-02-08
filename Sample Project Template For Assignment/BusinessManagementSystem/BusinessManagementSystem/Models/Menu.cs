using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessManagementSystem.Models
{
    public class Menu:BaseEntity
    {
        public int Id { get; set; }
        //public int MenuId { get; set; }
        public int Parent { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Url { get; set; }
        [Required]
        public int Sort { get; set; }
        [Required]
        public bool Status { get; set; }
        [Required]
        public string Icon { get; set; }

        [ValidateNever]
        [NotMapped]
        public Multiselect Multiselect { get; set; }
        [ValidateNever]
        public virtual ICollection<MenuRole> MenuRoles { get; set; }
    }

    public class MenuEntityConfiguration : IEntityTypeConfiguration<Menu>

    {
        public void Configure(EntityTypeBuilder<Menu> builder)
        {
            builder.Property(x => x.Name).HasColumnType("varchar(50)");
            builder.Property(x => x.Url).HasColumnType("varchar(255)");
            builder.Property(x => x.Icon).HasColumnType("varchar(150)");

        }
    }
}
