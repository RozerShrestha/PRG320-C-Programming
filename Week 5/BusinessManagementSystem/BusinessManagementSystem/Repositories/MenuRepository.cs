using BusinessManagementSystem.Data;
using BusinessManagementSystem.Dto;
using BusinessManagementSystem.Models;
using BusinessManagementSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq;
using System.Net;

namespace BusinessManagementSystem.Repositories
{

    public class MenuRepository : GenericRepository<Menu>, IMenu
    {
        private readonly ApplicationDBContext _db;
        public ResponseDto<Menu> _responseDto;

        public MenuRepository(ApplicationDBContext db) : base(db)
        {
            _responseDto = new ResponseDto<Menu>();
            _db = db;
        }
        public dynamic ParentList()
        {
            var parentList = _db.Menus.Where(p => p.Parent == 0 && p.Status == true).Select(p => new { Parent = p.Id, p.Name }).ToList();
            parentList.Add(new { Parent = 0, Name = "Main Parent" });
            parentList.Sort((a, b) => a.Parent.CompareTo(b.Parent));
            return parentList;
        }
        public Multiselect RoleList()
        {
            var roleLIst = _db.Roles.Select(p=> new { Id=p.Id, Name=p.Name }).ToList();

            var roleLists = new Multiselect();
            var listItems = new List<SelectListItem>();
            foreach (var role in roleLIst)
            {
                listItems.Add(new SelectListItem { Value = role.Id.ToString(), Text = role.Name });
            }
            roleLists.Items = listItems;
            return roleLists;
        }
        public ResponseDto<Menu> CreateMenu(Menu menu)
        {
            try
            {
                List<Role> selectedRoles = null;

                var selectedRoles1 = menu.Multiselect.SelectedItems.ToList();
                selectedRoles = _db.Roles.Where(p => selectedRoles1.Contains(p.Id)).ToList();
                _db.Database.BeginTransaction();
                foreach (var role in selectedRoles)
                {
                    MenuRole menuRole = new()
                    {
                        Role = role,
                        Menu = menu
                    };
                    _db.MenuRoles.Add(menuRole);
                }
                _db.SaveChanges();
                _db.Database.CommitTransaction();

            }
            catch (Exception ex)
            {
                _responseDto.StatusCode = HttpStatusCode.InternalServerError;
                _responseDto.Message = ex.ToString();
                _db.Database.RollbackTransaction();
                
            }
            return _responseDto;
        }

        public ResponseDto<Menu> GetMenuById(int id)
        {
            return _responseDto;
        }


        public ResponseDto<Menu> UpdateMenu(Menu menu)
        {
            try
            {
                _db.Database.BeginTransaction();
                //_db.MenuRoles.RemoveRange(menu.MenuRoles);

                var previousMenuRoles = _db.Menus.Include(m => m.MenuRoles).Where(p => p.Id == menu.Id).SingleOrDefault();

                List<Role> selectedRoles = null;
                var selectedRoles1 = menu.Multiselect.SelectedItems.ToList();
                selectedRoles = _db.Roles.Where(p => selectedRoles1.Contains(p.Id)).ToList();

                _db.MenuRoles.RemoveRange(previousMenuRoles.MenuRoles);

                foreach (var role in selectedRoles)
                {
                    MenuRole menuRole = new()
                    {
                        RoleId = role.Id,
                        MenuId = menu.Id
                    };
                    _db.MenuRoles.AddRange(menuRole);
                    //_db.SaveChanges();
                }

                var menuToUpdate = _db.Menus.Where(m => m.Id == menu.Id).SingleOrDefault();
                _db.Entry(menuToUpdate).CurrentValues.SetValues(menu);
                _db.Entry(menuToUpdate).State = EntityState.Modified;
                _db.SaveChanges();
                _db.Database.CommitTransaction();
            }
            catch (Exception ex)
            {
                _responseDto.StatusCode = HttpStatusCode.InternalServerError;
                _responseDto.Message = ex.ToString();
                _db.Database.RollbackTransaction();
            }
            
            return _responseDto;
        }

        
    }
}
