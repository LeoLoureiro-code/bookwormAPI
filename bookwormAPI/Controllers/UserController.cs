using bookwormAPI.DTO;
using bookwormAPI.EF.DataAccess.Models;
using bookwormAPI.EF.DataAccess.Repositories;
using bookwormAPI.EF.DataAccess.Repositories.Interfaces;
using bookwormAPI.EF.DataAccess.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace bookwormAPI.Controllers
{
    [Route("Bookworm/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUserRepository _userRepository;
        private readonly PasswordService _passwordService;

        public UserController(IUserRepository userRepository, PasswordService passwordService)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
        }

        //Get: api/Users
        [HttpGet("all-users")]
        public async Task<ActionResult<IEnumerable<User>>> GetAllusers()
        {
            try
            {
                var users = await _userRepository.GetAllUsers();
                if (users == null || !users.Any())
                {
                    return NotFound("No users Found");
                }

                return Ok(users);
            }
            catch (Exception ex)
            {
                return Problem(
                    detail: ex.Message,
                    title: "An error occurred while fetching users.",
                    statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        //Get:api/Users/5

        [HttpGet("find-by-id/{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            try
            {
                var user = await _userRepository.GetUserById(id);

                if (user == null)
                {
                    return NotFound(
                        new
                        {
                            Message = $"User with ID {id} was not found."

                        });
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                return Problem(
                    detail: ex.Message,
                    title: "An error occurred while fetching user.",
                    statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("find-by-credentials")]

        public async Task<ActionResult<User>> GetUserByCredentials([FromBody] UserDTO user)
        {
            try
            {
                var existingUser = await _userRepository.GetUserByNameAndPasswordAsync(user.Email, user.Password);

                if (existingUser == null)
                {
                    throw new Exception("User not found.");
                }

                return Ok(existingUser);
            }
            catch (Exception ex)
            {
                return Problem(
                   detail: ex.Message,
                   title: "An error occurred while fetching user.",
                   statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        //POST: api/Users
        [HttpPost("create-user")]
        public async Task<ActionResult> CreateUser([FromBody] UserDTO user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new
                    {
                        Message = "Email and password are required."
                    });
                }
                var createdUser = await _userRepository.CreateUser(user);

                return CreatedAtAction(nameof(GetUser), new { id = createdUser.UserId }, createdUser);
            }
            catch (Exception ex)
            {
                return Problem(
                    detail: ex.Message,
                    title: "An error occurred while creating the user.",
                    statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        //PUT: api/users/5
        [HttpPut("update-user/{id}")]
        public async Task<ActionResult> UpdateUser(int id, [FromBody] User user)
        {
            try
            {
                // Check if route ID matches body ID (if applicable)
                if (user.UserId != 0 && user.UserId != id)
                {
                    return BadRequest("User ID in the body does not match URL.");
                }

                // Optional: Basic validation
                if (string.IsNullOrWhiteSpace(user.UserName) || string.IsNullOrWhiteSpace(user.UserPasswordHash))
                {
                    return BadRequest("Email and password cannot be empty.");
                }

                // Hash the password before updating
                user.UserPasswordHash = _passwordService.HashPassword(user.UserPasswordHash);

                var updated = await _userRepository.UpdateUser(id, user.UserName, user.UserPasswordHash);

                if (updated == null)
                {
                    return NotFound($"User with ID {id} not found.");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return Problem(
                    detail: ex.Message,
                    title: "An error occurred while updating the user.",
                    statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        // DELETE: api/Users/5
        [HttpDelete("delete-user/{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            try
            {
                var user = await _userRepository.GetUserById(id);
                if (user == null)
                {
                    return NotFound($"User with ID {id} not found.");
                }

                await _userRepository.DeleteUser(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return Problem(
                    detail: ex.Message,
                    title: "An error occurred while deleting the user.",
                    statusCode: StatusCodes.Status500InternalServerError);
            }
        }
    }
}
