using Work.Interfaces;
using Work.Models;

namespace Work.Implementations
{
    public class UserData : IUserData
    {
        public Dictionary<Guid, User> Users { get; private set; }

        public UserData(Dictionary<Guid, User> users)
        {
            Users = users;
        }

        public UserData(int seed)
        {
            Users = new Dictionary<Guid, User>();
            for (int i = 0; i < seed; i++)
            {
                var user = new User
                {
                    UserId = Guid.NewGuid(),
                    UserName = $"User {i}",
                    Birthday = DateTime.Now.AddYears(-i)
                };
                Users.Add(user.UserId, user);
            }
        }
    }
}
