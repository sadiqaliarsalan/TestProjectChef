using Work.Controllers.Models.DTOs;

namespace Work.Controllers.Models.Extensions
{
    public static class UserDtoExtensions
    {
        public static bool IsValid(this UserDto userDto, out string validationMessage)
        {
            if (string.IsNullOrEmpty(userDto.Name))
            {
                validationMessage = "Name is required";
                return false;
            }

            if (userDto.Birthdate >= DateTime.Today)
            {
                validationMessage = "Birthdate must be in the past";
                return false;
            }

            validationMessage = string.Empty;
            return true;
        }
    }
}
