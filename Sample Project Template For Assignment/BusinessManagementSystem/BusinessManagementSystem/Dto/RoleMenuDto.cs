using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessManagementSystem.Dto
{ 
    public class RoleMenuDto
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public bool Check { get; set; }
        public List<Children1> Children { get; set; }
    }
    public class Children1
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public bool Check { get; set; }
        public List<Children2> Children { get; set; }
    }
    public class Children2
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public bool Check { get; set; }
    }
}
