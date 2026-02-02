using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace StudentPortal.Web.Models
{
    public class Student
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public bool Subcribed { get; set; }

    }

    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.Property(x => x.Name).HasColumnType("varchar(255)");
            builder.Property(x => x.Email).HasColumnType("varchar(255)");
            builder.Property(x => x.PhoneNumber).HasColumnType("varchar(10)");
            builder.Property(x => x.Address).HasColumnType("varchar(255)");
        }
    }
}
