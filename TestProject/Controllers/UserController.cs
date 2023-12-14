using Microsoft.AspNetCore.Mvc;
using Work.Controllers.Models.DTOs;
using Work.Models;
using Newtonsoft.Json;
using Work.Interfaces;
using Work.Controllers.Models.Mappers;
using Work.Controllers.Models.DTOs.Extensions;

namespace Work.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserManager<User, Guid> _userManager;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserManager<User, Guid> userManager, ILogger<UserController> logger)
        {
            _userManager = userManager;
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
            _logger.LogInformation($"Getting user called with Id {id}");
            var user = _userManager.Read(id);
            if (user == null)
            {
                _logger.LogWarning($"User not found with Id {id}");
                return NotFound(new { message = $"User not found with Id {id}" });
            }

            return Ok(user);
        }

        /// <summary>
        /// Gets a user by its Id
        /// </summary>
        /// <response code="200">Returns the requested user</response>
        /// <response code="404">If the user is not found</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<User>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetAll()
        {
            _logger.LogInformation($"Get all users called");
            var users = _userManager.ReadAll();
            if (!users.Any())
            {
                _logger.LogWarning($"Users not found");
                return NotFound(new { message = $"Users not found" });
            }

            return Ok(users);
        }

        /// <summary>
        /// Create a new user
        /// </summary>
        /// <param name="userDto">The user dto</param>
        /// <response code="200">User created successfully</response>
        /// <response code="400">Bad request if user data is invalid</response>
        /// <response code="409">Conflict if user already exists</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public IActionResult Post(UserDto userDto)
        {
            _logger.LogInformation($"Posting user caleed with {JsonConvert.SerializeObject(userDto)}");

            if (!userDto.IsValidDto(out var validationMessage))
            {
                _logger.LogError($"Error validating user {validationMessage}");
                return BadRequest(validationMessage);
            }

            try
            {
                var user = UserMapper.MapToUser(userDto);
                _userManager.Create(user);
                return Ok(new { message = "User created successfully" });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Error creating user", ex.Message);
                return Conflict(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Update an existing user
        /// </summary>
        /// <param name="userDto">The user dto with updated information</param>
        /// <response code="200">User updated successfully</response>
        /// <response code="404">If the user to be updated is not found</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Put(UserDto userDto)
        {
            _logger.LogInformation($"Updating user called with {JsonConvert.SerializeObject(userDto)}");

            var user = _userManager.Read(userDto.UserId);
            if (user == null)
            {
                _logger.LogWarning($"User not found with Id {userDto.UserId}");
                return NotFound(new { message = $"User not found with Id {userDto.UserId}" });
            }

            var shouldUpdate = false;

            if (!string.IsNullOrEmpty(userDto.UserName) && !userDto.UserName.Equals(user.UserName))
            {
                user.UserName = userDto.UserName;
                shouldUpdate = true;
            }

            if (userDto.Birthday != null && userDto.Birthday.IsValidBirthDate())
            {
                user.Birthday = (DateTime)userDto.Birthday;
                shouldUpdate = true;
            }

            if (shouldUpdate)
            {
                _userManager.Update(user);
                return Ok(new { message = "User updated successfully", user });
            }

            return Ok(new { message = "Nothing to update on this user", user });
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
            _logger.LogInformation($"Deleting user called with Id {id}");

            var user = _userManager.Read(id);
            if (user == null)
            {
                _logger.LogWarning($"User not found with Id {id}");
                return NotFound(new { message = $"User not found with Id {id}" });
            }

            _userManager.Remove(user);
            return Ok(new { message = $"User deleted successfully with Id {id}" });
        }
    }
}