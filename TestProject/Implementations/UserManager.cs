using Work.Interfaces;
using Work.Models;

namespace Work.Implementation
{
    public class UserManager : IUserManager<User, Guid>
    {
        private readonly IUserData _userData;

        public UserManager(IUserData userData)
        {
            _userData = userData;
        }

        public void Create(User user)
        {
            if (_userData.Users.ContainsKey(user.UserId))
            {
                throw new InvalidOperationException("User with the same Id already exists");
            }

            _userData.Users.Add(user.UserId, user);
        }

        public User? Read(Guid userId)
        { 
            if (_userData.Users != null && _userData.Users.TryGetValue(userId, out var user))
            {
                return user;
            }

            return null;
        }

        public List<User> ReadAll()
        {
            if (_userData.Users != null && _userData.Users.Count > 0)
            {
                return _userData.Users.Values.ToList();
            }

            return new List<User>(); // empty list
        }

        public void Update(User user)
        {
            _userData.Users[user.UserId] = user;
        }

        public void Remove(User user)
        {
            _userData.Users.Remove(user.UserId);
        }
    }
}
