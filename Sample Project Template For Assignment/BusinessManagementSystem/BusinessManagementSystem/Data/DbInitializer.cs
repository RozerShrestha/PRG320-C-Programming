using BusinessManagementSystem.Dto;
using BusinessManagementSystem.Models;
using BusinessManagementSystem.Services;
using BusinessManagementSystem.Utility;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace BusinessManagementSystem.Data
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDBContext _db;
        public DbInitializer(ApplicationDBContext db)
        {
            _db = db;
        }
        public async void Initialize()
        {
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception)
            {
                throw;
            }

            //create role if not created
            var isRoleExist = _db.Roles.Any();
            if (!isRoleExist)
            {
                List<Role> roles =
                [
                    new Role { Id = 1, CreatedBy="System", UpdatedBy="System", Name = SD.Role_Superadmin },
                    new Role { Id = 2, CreatedBy = "System", UpdatedBy = "System", Name = SD.Role_Admin },
                    new Role { Id = 3, CreatedBy="System",UpdatedBy="System", Name = SD.Role_Employee },
                    new Role { Id = 4, CreatedBy="System",UpdatedBy="System", Name = SD.Role_HR }
                ];
                _db.AddRange(roles);
                _db.SaveChanges();
            }

            //create user if not created
            var isUserCreated = _db.Users.Any();
            if (!isUserCreated)
            {
                UserRole ur = new UserRole();
                User u = new User
                {
                    Guid = Helper.Helpers.GenerateGUID(),
                    UserName = "rozer.shrestha",
                    Email = "rozer.shresthatest@gmail.com",
                    HashPassword = "vNWsg9wG82FOVlKYm4fJNkTSysuUuGoeuCNL/oYbwn4=", //12345678
                    Salt = "x9MC6J+9dMJ06q0OP/T4/w==",
                    FullName = "Rozer Shrestha",
                    Address = "Bhimsensthan",
                    DateOfBirth = DateOnly.Parse("1991/03/01"),
                    PhoneNumber = "9818136462",
                    Gender = "Male",
                    Occupation = "Chief Operating Officer",
                    Skills="Here goes the skill",
                    Notes="Here goes the Notes",
                    FacebookLink="https://youtube.com",
                    InstagramLink= "https://youtube.com",
                    TiktokLink= "https://youtube.com",
                    CreatedBy = "System",
                    UpdatedBy = "System"
                };

                ur.User = u;
                ur.RoleId = 1;
                _db.Add(ur);
                _db.SaveChanges();
            }

            //create BasicConfiguration
            var isBasicConfiguration = _db.BasicConfigurations.Any();
            if (!isBasicConfiguration)
            {
                BasicConfiguration basicConfiguration = new BasicConfiguration
                {
                    EmailAlias = "Email alias",
                    Email = "employer@gmail.com",
                    Password = "Not Required",
                    HostName = "hostname",
                    Port = 25,
                    ApplicationTitle = "Sample Application Title",
                    EmployerName = "Employer Name",
                    EmployerEmailAddress = "EmployerEmail@gmail.com",
                    EmployerAddress = "EMployer Address",
                    CreatedBy = "System",
                    UpdatedBy = "System"

                };
                _db.Add(basicConfiguration);
                _db.SaveChanges();
            }

            //create Menu and menurole
            var isMenuCreated = _db.Menus.Any();
            if (!isMenuCreated)
            {
                List<Menu> menus = [
                    new Menu { Parent = 0,     Name ="Configurations",          Url ="#",                    Sort = 1, Status = true,   CreatedBy="NULL", UpdatedBy="NULL",     Icon ="fas fa-cogs"},
                    new Menu { Parent = 1,     Name ="Basic Configuration",     Url ="/BasicConfiguration",  Sort = 1, Status = true,   CreatedBy="System", UpdatedBy="System",     Icon ="fa"},
                    new Menu { Parent = 1,     Name ="Role",                    Url ="/Role",                Sort = 1, Status = true,   CreatedBy="System", UpdatedBy="System",     Icon ="fa"},
                    new Menu { Parent = 1,     Name ="Menu",                    Url ="/Menu",               Sort = 2, Status = true,   CreatedBy="System", UpdatedBy="System",     Icon ="fa"},
                    new Menu { Parent = 0,     Name ="Users",                   Url ="#",                    Sort = 2, Status = true,   CreatedBy="NULL", UpdatedBy="NULL",     Icon ="fas fa-user"},
                    new Menu { Parent = 5,  Name ="Create User",             Url ="/Users/Create", Sort = 1, Status = true, CreatedBy="rozer.shrestha", UpdatedBy="rozer.shrestha", Icon ="fa"},
                    new Menu { Parent = 5, Name ="All Users", Url ="/Users/Index", Sort = 2, Status = true, CreatedBy="rozer.shrestha", UpdatedBy="rozer.shrestha", Icon ="fa"},
                    new Menu { Parent = 5, Name ="My Profile", Url ="/Users/Detail", Sort = 3, Status = true, CreatedBy="rozer.shrestha", UpdatedBy="rozer.shrestha", Icon ="fa"}
                ];


                var roles = _db.Roles.ToList();

                _db.Database.BeginTransaction();
                foreach (var role in roles)
                {
                    foreach (var menu in menus)
                    {
                        MenuRole mr = new()
                        {
                            Role = role,
                            Menu = menu
                        };
                        _db.MenuRoles.Add(mr);
                    }
                }
                _db.SaveChanges();
                _db.Database.CommitTransaction();
            }
        }
    }
}
