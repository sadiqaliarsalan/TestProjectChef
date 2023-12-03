using NUnit.Framework;
using Work.Data;
using Work.Implementation;
using Work.Models;

namespace Work.Test.Repositories
{
    [TestFixture]
    public class UserRepositoryTest
    {
        private MockDatabase _mockDatabase;
        private UserRepository _userRepository;

        [SetUp]
        public void Setup()
        {
            // arrange
            _mockDatabase = new MockDatabase(new Dictionary<Guid, User>());
            _userRepository = new UserRepository(_mockDatabase);
        }

        [Test]
        public void Create_NewUser_ShouldCreateUser_Test()
        {
            // arrange
            var newUser = new User { UserId = Guid.NewGuid(), UserName = "TestUser", Birthday = DateTime.Today };

            // act
            _userRepository.Create(newUser);

            // assert
            Assert.IsTrue(_mockDatabase.Users.ContainsKey(newUser.UserId));
        }

        [Test]
        public void Create_ExistingUser_ShouldThrowInvalidOperationException_Test()
        {
            // arrange
            var existingUser = new User { UserId = Guid.NewGuid(), UserName = "ExistingUser", Birthday = DateTime.Today };

            // act
            _userRepository.Create(existingUser);

            // assert
            Assert.Throws<InvalidOperationException>(() => _userRepository.Create(existingUser));
        }

        [Test]
        public void Read_ExistingUser_ShouldReturnValidUser_Test()
        {
            // arrange
            var user = new User { UserId = Guid.NewGuid(), UserName = "TestUser", Birthday = DateTime.Today };
            _mockDatabase.Users.Add(user.UserId, user);

            // act
            var result = _userRepository.Read(user.UserId);

            // assert
            Assert.AreEqual(user, result);
        }

        [Test]
        public void Read_NonExistingUser_ShouldReturnNull_Test()
        {
            // arrange and act
            var result = _userRepository.Read(Guid.NewGuid());

            // assert
            Assert.IsNull(result);
        }

        [Test]
        public void Read_UsersIsNull_ShouldReturnNull_Test()
        {
            // arrange
            var emptyMockedDb = new MockDatabase(null);
            _userRepository = new UserRepository(emptyMockedDb);

            // act
            var result = _userRepository.Read(Guid.NewGuid());

            // assert
            Assert.IsNull(result);
        }

        [Test]
        public void Update_ExistingUser_ShouldBeUpdated_Test()
        {
            // arrange
            var user = new User { UserId = Guid.NewGuid(), UserName = "TestUser", Birthday = DateTime.Today };
            _mockDatabase.Users.Add(user.UserId, user);

            Assert.AreEqual("TestUser", _mockDatabase.Users[user.UserId].UserName);

            user.UserName = "UpdatedUser";

            // act
            var result = _userRepository.Update(user);

            // assert
            Assert.IsTrue(result);
            Assert.AreEqual("UpdatedUser", _mockDatabase.Users[user.UserId].UserName);
        }

        [Test]
        public void Update_NonExistingUser_ShouldReturnFalse_Test()
        {
            // arrange
            var user = new User { UserId = Guid.NewGuid(), UserName = "NonExistingUser", Birthday = DateTime.Today };

            // act
            var result = _userRepository.Update(user);

            // assert
            Assert.IsFalse(result);
        }

        [Test]
        public void Remove_ExistingUser_ShouldReturnTrue_Test()
        {
            // arrange
            var user = new User { UserId = Guid.NewGuid(), UserName = "TestUser", Birthday = DateTime.Today };
            _mockDatabase.Users.Add(user.UserId, user);

            // act
            var result = _userRepository.Remove(user);

            // assert
            Assert.IsTrue(result);
            Assert.IsFalse(_mockDatabase.Users.ContainsKey(user.UserId));
        }

        [Test]
        public void Remove_NonExistingUser_ShouldReturnFalse_Test()
        {
            // arrange
            var user = new User { UserId = Guid.NewGuid(), UserName = "NonExistingUser", Birthday = DateTime.Today };

            // act
            var result = _userRepository.Remove(user);

            // assert
            Assert.IsFalse(result);
        }
    }
}
