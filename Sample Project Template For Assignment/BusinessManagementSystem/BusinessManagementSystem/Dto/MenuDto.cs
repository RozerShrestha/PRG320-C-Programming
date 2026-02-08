using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessManagementSystem.Dto
{
    public class MenuDto
    {
        public int MenuId { get; set; }
        public int Parent { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public int Sort { get; set; }
        public string Icon { get; set; }
        public bool Status { get; set; }
        public List<SubMenu> SubMenu {get;set;}
    }

    public class SubMenu
    {
        public int MenuId { get; set; }
        public int Parent { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public int Sort { get; set; }
        public string Icon { get; set; }
    }
}
