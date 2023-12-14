using Newtonsoft.Json;

namespace Work.Controllers.Models.DTOs
{
    // UserDto class. To be used in PUT and Post requests
    public class UserDto
    {
        public Guid UserId { get; set; }

        public string? UserName { get; set; }

        public DateTime? Birthday { get; set; }
    }
}
