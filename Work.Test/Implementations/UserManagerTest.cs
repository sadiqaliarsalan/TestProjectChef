using Moq;
using NUnit.Framework;
using Work.Implementation;
using Work.Interfaces;
using Work.Models;

namespace Work.Test.Implementations
{
    [TestFixture]
    public class UserManagerTest
    {
        private Mock<IUserData> _mockedUserData;
        private UserManager _userManager;

        [SetUp]
        public void Setup()
        {
            _mockedUserData = new Mock<IUserData>();
            _userManager = new UserManager(_mockedUserData.Object);

            // Set up the mocked Users dictionary
            var users = new Dictionary<Guid, User>();
            _mockedUserData.Setup(m => m.Users).Returns(users);
        }

        [Test]
        public void Create_NewUser_ShouldCreateUser_Test()
        {
            // arrange
            var newUser = new User { UserId = Guid.NewGuid(), UserName = "TestUser", Birthday = DateTime.Today.AddDays(-1) };
            var users = new Dictionary<Guid, User>();
            _mockedUserData.Setup(m => m.Users).Returns(users);

            // act
            _userManager.Create(newUser);

            // assert
            Assert.IsTrue(users.ContainsKey(newUser.UserId));
            Assert.AreEqual(newUser, users[newUser.UserId]);
        }


        [Test]
        public void Create_ExistingUser_ShouldThrowInvalidOperationException_Test()
        {
            // arrange
            var existingUserId = Guid.NewGuid();
            var existingUser = new User { UserId = existingUserId, UserName = "ExistingUser", Birthday = DateTime.Today.AddDays(-1) };
            var users = new Dictionary<Guid, User> { { existingUserId, existingUser } };
            _mockedUserData.Setup(m => m.Users).Returns(users);

            // act & assert
            Assert.Throws<InvalidOperationException>(() => _userManager.Create(existingUser));
        }

        [Test]
        public void Read_ExistingUser_ShouldReturnValidUser_Test()
        {
            // arrange
            var userId = Guid.NewGuid();
            var user = new User { UserId = userId, UserName = "TestUser", Birthday = DateTime.Today.AddDays(-1) };
            var users = new Dictionary<Guid, User> { { userId, user } };
            _mockedUserData.Setup(m => m.Users).Returns(users);

            // act
            var result = _userManager.Read(userId);

            // assert
            Assert.AreEqual(user, result);
        }

        [Test]
        public void ReadAll_ShouldReturnValidUsers_Test()
        {
            // arrange
            var userId = Guid.NewGuid();
            var user = new User { UserId = userId, UserName = "TestUser", Birthday = DateTime.Today.AddDays(-1) };
            var users = new Dictionary<Guid, User> { { userId, user } };
            _mockedUserData.Setup(m => m.Users).Returns(users);

            // act
            var result = _userManager.ReadAll();

            // assert
            Assert.NotNull(result);
        }

        [Test]
        public void Read_NonExistingUser_ShouldReturnNull_Test()
        {
            // arrange
            var users = new Dictionary<Guid, User>();
            _mockedUserData.Setup(m => m.Users).Returns(users);

            // act
            var result = _userManager.Read(Guid.NewGuid());

            // assert
            Assert.IsNull(result);
        }

        [Test]
        public void Update_ShouldBeUpdated_Test()
        {
            // arrange
            var userId = Guid.NewGuid();
            var user = new User { UserId = userId, UserName = "TestUser", Birthday = DateTime.Today };
            var users = new Dictionary<Guid, User> { { userId, user } };
            _mockedUserData.Setup(m => m.Users).Returns(users);

            var updatedUser = new User { UserId = userId, UserName = "UpdatedUser", Birthday = DateTime.Today };

            // act
            _userManager.Update(updatedUser);

            // assert
            Assert.IsTrue(users.ContainsKey(userId));
            Assert.AreEqual("UpdatedUser", users[userId].UserName);
        }

        [Test]
        public void Remove_ShouldBeRemoved_Test()
        {
            // arrange
            var userId = Guid.NewGuid();
            var user = new User { UserId = userId, UserName = "TestUser", Birthday = DateTime.Today };
            var users = new Dictionary<Guid, User> { { userId, user } };
            _mockedUserData.Setup(m => m.Users).Returns(users);

            // act
            _userManager.Remove(user);

            // assert
            Assert.IsFalse(users.ContainsKey(userId));
        }

        [Test]
        public void Remove_NonExistingUser_ShouldNotRemove_Test()
        {
            // arrange
            var userId = Guid.NewGuid();
            var user = new User { UserId = userId, UserName = "NonExistingUser", Birthday = DateTime.Today };
            var users = new Dictionary<Guid, User>();
            _mockedUserData.Setup(m => m.Users).Returns(users);

            // act
            _userManager.Remove(user);

            // assert
            Assert.IsFalse(users.ContainsKey(userId));
        }
    }
}
