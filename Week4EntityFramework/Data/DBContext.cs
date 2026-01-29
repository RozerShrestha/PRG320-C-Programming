using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Week4EntityFramework.Models;

namespace Week4EntityFramework.Data
{
    public class SchoolContext:DbContext
    {
        public virtual DbSet<Student> Students { get; set; }
        public DbSet<Grade> Grades { get; set; }

        IConfiguration appConfig;
        public SchoolContext()
        {
            
        }
        public SchoolContext(IConfiguration appConfig)
        {
            this.appConfig = appConfig;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var configuration = GetConfiguration();
            var con = new SqlConnection(configuration.GetSection("ConnectionStrings").GetSection("SchoolDbConnectionString").Value);
            optionsBuilder.UseSqlServer(con.ConnectionString);
        }

        public IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Student>();
        //    //modelBuilder.Entity<Student>().HasData(
        //    //    new Student { Id = 1, Name = "Rozer Shrestha", Address = "Basantapur", PhoneNumber = 9818181812 }
        //    //    );
        //}
    }
}
