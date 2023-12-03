namespace Work.Controllers.Models.DTOs
{
    // UserDto class. To be used in PUT and Post requests
    public class UserDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public DateTime Birthdate { get; set; }
    }
}
