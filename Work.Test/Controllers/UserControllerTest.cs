using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Work.Controllers;
using Work.Controllers.Models.DTOs;
using Work.Interfaces;
using Work.Models;

namespace Work.Test.Controllers
{
    [TestFixture]
    public class UserControllerTest
    {
        private Mock<IUserManager<User, Guid>> _mockUserRepository;
        private Mock<ILogger<UserController>> _mockLogger;
        private UserController _userController;

        [SetUp]
        public void Setup()
        {
            _mockUserRepository = new Mock<IUserManager<User, Guid>>();
            _mockLogger = new Mock<ILogger<UserController>>();
            _userController = new UserController(_mockUserRepository.Object, _mockLogger.Object);
        }

        [Test]
        public void Get_WhenUserExist_ShouldReturnValidUser_Test()
        {
            // arrange
            var userId = Guid.NewGuid();
            var expectedUser = new User()
            {
                UserId = userId,
                UserName = "test user",
                Birthday = new DateTime(2000, 01, 01)
            };

            _mockUserRepository.Setup(x => x.Read(userId)).Returns(expectedUser);

            // act
            var result = _userController.Get(userId);

            // assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);

            var userResult = okResult.Value as User;
            Assert.IsNotNull(userResult);
            Assert.AreEqual(expectedUser.UserId, userResult.UserId);
            Assert.AreEqual(expectedUser.UserName, userResult.UserName);
            Assert.AreEqual(expectedUser.Birthday, userResult.Birthday);
        }

        [Test]
        public void GetAll_ShouldReturnValidUsers_Test()
        {
            // arrange
            var userId = Guid.NewGuid();
            var expectedUsers = new List<User>()
            {
               new User()
               {
                    UserId = userId,
                    UserName = "test user",
                    Birthday = new DateTime(2000, 01, 01)
               }
            };

            _mockUserRepository.Setup(x => x.ReadAll()).Returns(expectedUsers);

            // act
            var result = _userController.GetAll();

            // assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);

            var usersResult = okResult.Value as List<User>;
            Assert.IsNotNull(usersResult);
            Assert.AreEqual(expectedUsers.FirstOrDefault().UserId, usersResult.FirstOrDefault().UserId);
            Assert.AreEqual(expectedUsers.FirstOrDefault().UserName, usersResult.FirstOrDefault().UserName);
            Assert.AreEqual(expectedUsers.FirstOrDefault().Birthday, usersResult.FirstOrDefault().Birthday);
        }

        [Test]
        public void Get_WhenUserDoesNotExist_ShouldReturnNotFound_Test()
        {
            // arrange
            var userId = Guid.NewGuid();
            _mockUserRepository.Setup(repo => repo.Read(userId)).Returns((User)null);

            // act
            var result = _userController.Get(userId);

            // assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
        }

        [Test]
        public void Post_WithValidUser_ShouldCreateUser_Test()
        {
            // arrange
            var userDto = new UserDto { UserId = Guid.NewGuid(), UserName = "test user", Birthday = new DateTime(2000, 01, 01) };
            _mockUserRepository.Setup(repo => repo.Create(It.IsAny<User>()));

            // act
            var result = _userController.Post(userDto);

            // assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public void Post_WithDuplicateUserId_ShouldReturnConflictResponse_Test()
        {
            // arrange
            var duplicateUserId = Guid.NewGuid();
            var userDto = new UserDto { UserId = duplicateUserId, UserName = "test user", Birthday = new DateTime(2000, 01, 01) };
            var existingUser = new User { UserId = duplicateUserId, UserName = "existing user", Birthday = new DateTime(1999, 01, 01) };

            var users = new Dictionary<Guid, User>
            {
                { duplicateUserId, existingUser } // Simulate existing user with the same id
            };

            _mockUserRepository.Setup(x => x.Create(It.IsAny<User>()))
                               .Callback<User>(u =>
                               {
                                   if (users.ContainsKey(u.UserId))
                                   {
                                       throw new InvalidOperationException("User with the same Id already exists");
                                   }
                                   users.Add(u.UserId, u);
                               });

            // act
            var result = _userController.Post(userDto);

            // assert
            Assert.IsInstanceOf<ConflictObjectResult>(result);
        }


        [Test]
        public void Post_InvalidUserDataBirthdateInFuture_ShouldReturnBadRequest_Test()
        {
            // arrange
            var userDto = new UserDto { UserId = Guid.NewGuid(), UserName = "test user", Birthday = new DateTime(2025, 01, 01) };

            // act
            var result = _userController.Post(userDto);

            // assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public void Put_WithExistingUser_ShouldUpdateUser_Test()
        {
            // arrange
            var userDto = new UserDto { UserId = Guid.NewGuid(), UserName = "test user", Birthday = new DateTime(2000, 01, 01) };
            _mockUserRepository.Setup(repo => repo.Read(userDto.UserId)).Returns(new User());

            // act
            var result = _userController.Put(userDto);

            // assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public void Put_WithNonExistingUser_ShouldReturnNotFound_Test()
        {
            // arrange
            var userDto = new UserDto { UserId = Guid.NewGuid(), UserName = "test user", Birthday = new DateTime(2000, 01, 01) };
            _mockUserRepository.Setup(repo => repo.Read(userDto.UserId)).Returns((User)null);

            // act
            var result = _userController.Put(userDto);

            // assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
        }

        [Test]
        public void Put_NoUserInDatabase_ShouldReturnNotFound_Test()
        {
            // arrange
            var userDto = new UserDto { UserId = Guid.NewGuid(), UserName = "test user", Birthday = new DateTime(2025, 01, 01) };

            // act
            var result = _userController.Put(userDto);

            // assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
        }

        [Test]
        public void Delete_WithExistingUser_ShouldDeleteUser_Test()
        {
            // arrange
            var userId = Guid.NewGuid();
            _mockUserRepository.Setup(repo => repo.Read(userId)).Returns(new User());

            // act
            var result = _userController.Delete(userId);

            // assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public void Delete_WithNonExistingUser_ShouldReturnNotFound_Test()
        {
            // arrange
            var userId = Guid.NewGuid();
            _mockUserRepository.Setup(repo => repo.Read(userId)).Returns((User)null);

            // act
            var result = _userController.Delete(userId);

            // assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
        }

    }
}
