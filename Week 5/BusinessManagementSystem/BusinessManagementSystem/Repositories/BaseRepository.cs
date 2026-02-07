using BusinessManagementSystem.Data;
using BusinessManagementSystem.Dto;
using BusinessManagementSystem.Models;
using BusinessManagementSystem.Services;
using System.Net;

namespace BusinessManagementSystem.Repositories
{
    public class BaseRepository : IBase
    {
        private readonly ApplicationDBContext _db;
        protected UserDto userDto;
        public BaseRepository(ApplicationDBContext db)
        {
            _db= db;
            userDto = new UserDto();
        }
        public List<MenuDto> MenuList(string roleName)
        {
            var menu = (from m in _db.Menus
                        join mr in _db.MenuRoles on m.Id equals mr.MenuId
                        join r in _db.Roles on mr.RoleId equals r.Id
                        where r.Name == roleName && m.Status == true
                        select m).ToList();

            var menuFilter = menu.Where(e => e.Parent == 0)
                .Select(m => new MenuDto
                {
                    MenuId = m.Id,
                    Parent = m.Parent,
                    Name = m.Name,
                    Icon = m.Icon,
                    Url = m.Url,
                    Sort = m.Sort,
                    SubMenu = menu.Where(x => x.Parent == m.Id)
                    .Select(s => new SubMenu
                    {
                        MenuId = s.Id,
                        Parent = s.Parent,
                        Name = s.Name,
                        Icon = s.Icon,
                        Url = s.Url,
                        Sort = s.Sort,
                    }).OrderBy(s => s.Sort).ToList()
                }).OrderBy(m => m.Sort).ToList();
            return menuFilter;
        }

        public UserDto UserDetail(string userName)
        {
            try
            {
                var userDetail = (from u in _db.Users
                                  join ur in _db.UserRoles on u.Id equals ur.UserId
                                  join r in _db.Roles on ur.RoleId equals r.Id
                                  where u.Email == userName
                                  select new
                                  {
                                      userId = u.Id,
                                      u.FullName,
                                      u.UserName,
                                      u.Email,
                                      u.PhoneNumber,
                                      r.Id,
                                      RoleName = r.Name

                                  }).Single();
                userDto.UserId = userDetail.userId;
                userDto.UserName = userDetail.UserName;
                userDto.Email = userDetail.Email;
                userDto.PhoneNumber = userDetail.PhoneNumber;
                userDto.RoleId = userDetail.Id;
                userDto.RoleName = userDetail.RoleName;
                userDto.FullName = userDetail.FullName;
            }
            catch (Exception)
            {
                userDto = null;
            }
            return userDto;
        }

        public dynamic RoleList()
        {
            var roleLIst = _db.Roles.Select(p => new { Id = p.Id, Name = p.Name }).ToList();
            return roleLIst;
        }
    }
}
