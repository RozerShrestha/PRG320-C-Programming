using Microsoft.EntityFrameworkCore;
using StudentPortal.Web.Models;

namespace StudentPortal.Web.Data
{
    public class ApplicationDbContext : DbContext
    {
        //Constructor
        //We will be using this constructor inside the program.cs file and pass DbContextOptions from program.cs file and injecting into the ApplicationDbContext
        //It act as a Bridge between database and application
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        //DbSet is a collection of a perticular type
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new StudentConfiguration());
        }
    }
}