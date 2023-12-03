using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using Work.Controllers.Models.DTOs;
using Work.Controllers.Models.Extensions;
using Work.Data;
using Work.Implementation;
using Work.Models;
using Newtonsoft.Json;
using Work.Interfaces;
using Work.Controllers.Models.Mappers;

namespace Work.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository<User, Guid> _userRepository;

        public UserController(IUserRepository<User, Guid> userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            var user = _userRepository.Read(id);
            if (user == null)
            {
                return NotFound(new { message = $"User not found with Id {id}" });
            }

            return Ok(user);
        }

        [HttpPost]
        public IActionResult Post(UserDto userDto)
        {
            if (!userDto.IsValid(out var validationMessage))
            {
                return BadRequest(validationMessage);
            }

            var user = UserMapper.MapToUser(userDto);
            _userRepository.Create(user);
            return Ok(new { message = "User created successfully" });
        }


        [HttpPut]
        public IActionResult Put(UserDto userDto)
        {
            var user = _userRepository.Read(userDto.Id);
            if (user == null)
            {
                return NotFound(new { message = $"User not found with Id {userDto.Id}" });
            }

            if (!userDto.IsValid(out var validationMessage))
            {
                return BadRequest(validationMessage);
            }

            user.UserName = userDto.Name;
            user.Birthday = userDto.Birthdate;
            _userRepository.Update(user);

            return Ok(new { message = "User updated successfully", user });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var user = _userRepository.Read(id);
            if (user == null)
            {
                return NotFound(new { message = $"User not found with Id {id}" });
            }

            _userRepository.Remove(user);
            return Ok(new { message = "User deleted successfully" });
        }
    }
}