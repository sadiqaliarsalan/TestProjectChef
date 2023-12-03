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
using Microsoft.Extensions.Logging;

namespace Work.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository<User, Guid> _userRepository;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserRepository<User, Guid> userRepository, ILogger<UserController> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        /// <summary>
        /// Gets a user by its Id
        /// </summary>
        /// <param name="id">The Id of the user to retrieve</param>
        /// <response code="200">Returns the requested user</response>
        /// <response code="404">If the user is not found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(User))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Get(Guid id)
        {
            _logger.LogInformation($"Getting user with Id {id}");
            var user = _userRepository.Read(id);
            if (user == null)
            {
                _logger.LogWarning($"User not found with Id {id}");
                return NotFound(new { message = $"User not found with Id {id}" });
            }

            return Ok(user);
        }

        /// <summary>
        /// Create a new user
        /// </summary>
        /// <param name="userDto">The user dto</param>
        /// <response code="200">User created successfully</response>
        /// <response code="400">Bad request if user data is invalid</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        /// <summary>
        /// Update an existing user
        /// </summary>
        /// <param name="userDto">The user dto with updated information</param>
        /// <response code="200">User updated successfully</response>
        /// <response code="400">Bad request if user data is invalid</response>
        /// <response code="404">If the user to be updated is not found</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Delete a user by its Id
        /// </summary>
        /// <param name="id">The Id of the user to be deleted</param>
        /// <response code="200">User deleted successfully</response>
        /// <response code="404">If the user to be deleted is not found</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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