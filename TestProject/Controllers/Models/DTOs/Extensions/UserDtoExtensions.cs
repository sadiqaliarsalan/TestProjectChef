using Work.Controllers.Models.DTOs;

namespace Work.Controllers.Models.DTOs.Extensions
{
    // UserDtoExtensions class. Used to validation purpose on UserDto object
    public static class UserDtoExtensions
    {
        public static bool IsValidDto(this UserDto userDto, out string validationMessage)
        {
            if (string.IsNullOrEmpty(userDto.UserName))
            {
                validationMessage = "Name is required";
                return false;
            }

            if (!userDto.Birthday.IsValidBirthDate())
            {
                validationMessage = "Birthdate must be in the past";
                return false;
            }

            validationMessage = string.Empty;
            return true;
        }

        public static bool IsValidBirthDate(this DateTime? birthDate)
        {
            if (birthDate == null || birthDate >= DateTime.Today)
            {
                return false;
            }

            return true;
        }
    }
}
