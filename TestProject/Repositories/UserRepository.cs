using Work.Data;
using Work.Interfaces;
using Work.Models;

namespace Work.Implementation
{
    // IUserRepository interface implementation
    public class UserRepository : IUserRepository<User, Guid>
    {
        private readonly MockDatabase _mockedDatabase;

        public UserRepository(MockDatabase mockedDatabase)
        {
            _mockedDatabase = mockedDatabase;
        }

        public void Create(User user)
        {
            if (_mockedDatabase.Users.ContainsKey(user.UserId))
            {
                throw new InvalidOperationException("User with the same Id already exists");
            }

            _mockedDatabase.Users.Add(user.UserId, user);
        }

        public User? Read(Guid userId)
        {
            if (_mockedDatabase.Users != null && _mockedDatabase.Users.TryGetValue(userId, out var user))
            {
                return user;
            }

            return null;
        }

        public bool Update(User user)
        {
            if (_mockedDatabase.Users != null && _mockedDatabase.Users.ContainsKey(user.UserId))
            {
                _mockedDatabase.Users[user.UserId] = user;
                return true;
            }

            return false;
        }

        public bool Remove(User user)
        {
            if(_mockedDatabase.Users != null && _mockedDatabase.Users.ContainsKey(user.UserId))
            {
                _mockedDatabase.Users.Remove(user.UserId);
                return true;
            }

            return false;
        }
    }
}
