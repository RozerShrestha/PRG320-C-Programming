namespace BusinessManagementSystem.Models
{
    public class ModalView
    {
        public ModalView()
        {
            
        }
        public ModalView(string title, string type, string message, string link)
        {
            Title = title;
            Type = type;
            Message = message;
            Link = link;

        }
        public string Title { get; set; }
        public string Type { get; set; }
        public string Message { get; set; }
        public string? Link  { get; set; }
    }
}
