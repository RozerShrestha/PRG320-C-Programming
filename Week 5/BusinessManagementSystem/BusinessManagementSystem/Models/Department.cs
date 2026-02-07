namespace BusinessManagementSystem.Models
{
    public class Department:BaseEntity
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public List<User> Users { get; set; }
    }
}
