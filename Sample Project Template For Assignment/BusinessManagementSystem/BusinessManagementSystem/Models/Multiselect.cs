using Microsoft.AspNetCore.Mvc.Rendering;

namespace BusinessManagementSystem.Models
{
    public class Multiselect
    {
        public IEnumerable<int> SelectedItems { get; set; }
        public IEnumerable<SelectListItem> Items { get; set; }
        public bool Selected { get; set; }
        public bool Disabled { get; set; }
    }
}
