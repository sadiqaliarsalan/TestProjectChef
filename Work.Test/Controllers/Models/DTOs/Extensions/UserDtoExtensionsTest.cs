using NUnit.Framework;
using Work.Controllers.Models.DTOs;
using Work.Controllers.Models.DTOs.Extensions;

namespace Work.Test.Controllers.Models.DTOs.Extensions
{
    [TestFixture]
    public class UserDtoExtensionsTest
    {
        [Test]
        public void IsValid_WithValidProperties_ShouldReturnTrue_Test()
        {
            // arrange
            var userDto = new UserDto
            {
                UserName = "Valid User",
                Birthday = new DateTime(2000, 01, 01)
            };

            // act
            var result = userDto.IsValidDto(out string validationMessage);

            // assert
            Assert.IsTrue(result);
            Assert.IsEmpty(validationMessage);
        }

        [Test]
        public void IsValid_EmptyName_ShouldReturnFalseWithMessage_Test()
        {
            // arrange
            var userDto = new UserDto
            {
                UserName = string.Empty,
                Birthday = new DateTime(2000, 01, 01)
            };

            // act
            var result = userDto.IsValidDto(out string validationMessage);

            // assert
            Assert.IsFalse(result);
            Assert.AreEqual("Name is required", validationMessage);
        }

        [Test]
        public void IsValid_FutureBirthdate_ShouldReturnFalseWithMessage_Test()
        {
            // arrange
            var userDto = new UserDto
            {
                UserName = "Valid User",
                Birthday = DateTime.Today.AddDays(1)
            };

            // act
            var result = userDto.IsValidDto(out string validationMessage);

            // assert
            Assert.IsFalse(result);
            Assert.AreEqual("Birthdate must be in the past", validationMessage);
        }

        [Test]
        public void IsValidBirthDate_PastBirthDate_ShouldReturnTrue_Test()
        {
            // arrange
            // act
            var result = UserDtoExtensions.IsValidBirthDate(DateTime.Today.AddDays(-1));

            // assert
            Assert.IsTrue(result);
        }

        [Test]
        public void IsValidBirthDate_FutureBirthdate_ShouldReturnFalse_Test()
        {
            // arrange
            // act
            var result = UserDtoExtensions.IsValidBirthDate(DateTime.Today.AddDays(1));

            // assert
            Assert.IsFalse(result);
        }

        [Test]
        public void IsValidBirthDate_Null_ShouldReturnFalse_Test()
        {
            // arrange
            // act
            var result = UserDtoExtensions.IsValidBirthDate(null);

            // assert
            Assert.IsFalse(result);
        }
    }
}
