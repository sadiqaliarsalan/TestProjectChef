using Work.Controllers.Models.DTOs;
using Work.Models;

namespace Work.Controllers.Models.Mappers
{
    public static class UserMapper
    {
        public static User MapToUser(UserDto userDto)
        {
            return new User
            {
                UserId = Guid.NewGuid(),
                UserName = userDto.Name,
                Birthday = userDto.Birthdate
            };
        }
    }
}
