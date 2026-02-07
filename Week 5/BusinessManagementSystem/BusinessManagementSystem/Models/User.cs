 using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BusinessManagementSystem.Models
{
    public class User:BaseEntity
    {
        public int Id { get; set; }
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Guid { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string FullName { get; set; } 
        public DateOnly DateOfBirth { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public string Address { get; set; }
        [StringLength(10, ErrorMessage = "Invalid PhoneNumber Number", MinimumLength = 10)]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Occupation { get; set; }
        [Required]
        public string HashPassword { get; set; }
        [Required]
        public bool Status { get; set; }
        public bool FirstPasswordReset { get; set; }
        [Required]
        public string Salt { get; set; }
        [NotMapped]
        public int RoleId { get; set; }

        public string? FacebookLink { get; set; }
        public string? InstagramLink { get; set; }
        public string? TiktokLink { get; set; }
        public string? ProfilePictureLink { get; set; }
        public string? Skills { get; set; }
        public string? Notes { get; set; }

        [JsonIgnore]
        public ICollection<UserRole> UserRoles { get; set; }

    }

    public class UserEntityConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasIndex(x => x.UserName).IsUnique();
            builder.Property(x => x.UserName).HasColumnType("varchar(20)");
            builder.HasIndex(x => x.Email).IsUnique();
            builder.Property(x => x.Email).HasColumnType("varchar(255)");
            builder.Property(x => x.FullName).HasColumnType("varchar(255)");
            builder.Property(x => x.Gender).HasColumnType("varchar(10)");
            builder.Property(x => x.Address).HasColumnType("varchar(255)");
            builder.HasIndex(x=>x.PhoneNumber).IsUnique();
            builder.Property(x => x.PhoneNumber).HasColumnType("varchar(10)");
            builder.Property(x => x.UserName).HasColumnType("varchar(150)");
            builder.Property(x => x.Email).HasColumnType("varchar(150)");
            builder.Property(x => x.HashPassword).HasColumnType("varchar(255)");
            builder.Property(x => x.Salt).HasColumnType("varchar(255)");
            builder.Property(x => x.FacebookLink).HasColumnType("varchar(255)");
            builder.Property(x => x.InstagramLink).HasColumnType("varchar(255)");
            builder.Property(x => x.TiktokLink).HasColumnType("varchar(255)");
            builder.Property(x => x.ProfilePictureLink).HasColumnType("varchar(255)");
            builder.Property(x => x.Skills).HasColumnType("varchar(255)");
            builder.Property(x => x.Notes).HasColumnType("varchar(255)");
        }
    }
}
