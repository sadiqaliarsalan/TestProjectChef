namespace Work.Models
{
    // User model
    public class User
    {
        public Guid UserId { get; set; }

        public string UserName { get; set; }

        public DateTime Birthday { get; set; }
    }
}