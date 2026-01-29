using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Week4EntityFramework.Models;

namespace Week4EntityFramework.Data
{
    public class DbInitializer:IDbInitializer
    {
        public void Initialize()
        {
            using (var context = new SchoolContext())
            {
               var dbCreated = context.Database.EnsureCreated(); //creates db if not exist
                //create entity objects
                var grd1 = new Grade()
                {
                    GradeName = "1st Grade"
                };
                var std1 = new Student()
                {
                    FirstName = "Rozer",
                    LastName = "Shrestha",
                    Grade = grd1
                };
                //add entity to the context
                context.Students.Add(std1);

                var list = context.Students.Where(x => x.FirstName.Equals("ss")).ToList();
                list.ForEach(x => x.FirstName.Equals("ss"));
                context.SaveChanges();

                var list1 = context.Students.Where(x => x.FirstName.Equals("ss")).ExecuteUpdate(x => x.SetProperty(y => y.FirstName, "xx"));
                var list2 = context.Students.Where(x => x.FirstName.Equals("ss"));


                




                //save data to the database tables
                context.SaveChanges();

                //retrieve all the students from the database
                foreach (var s in context.Students)
                {
                    Console.WriteLine($"First Name: {s.FirstName}, Last Name: {s.LastName}");
                }

            }
        }
    }
}
