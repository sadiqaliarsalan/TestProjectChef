using NUnit.Framework;
using Work.Controllers.Models.DTOs;

namespace Work.Test.Controllers.Models.UserMapper
{
    [TestFixture]
    public class UserMapperTest
    {
        [Test]
        public void MapToUser_WhenCalled_ShouldReturnValidUser_Test()
        {
            // arrange
            var userDto = new UserDto
            {
                UserName = "Test User",
                Birthday = new DateTime(2000, 1, 1)
            };

            // act
            var result = Work.Controllers.Models.Mappers.UserMapper.MapToUser(userDto);

            // assert
            Assert.IsNotNull(result.UserId);
            Assert.AreEqual(userDto.UserName, result.UserName);
            Assert.AreEqual(userDto.Birthday, result.Birthday);
        }
    }
}
