using Microsoft.AspNetCore.Mvc;
using PersonInfo.Models;

namespace PersonInfo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet("all", Name = "GetAllUsers")]
        [ProducesResponseType(typeof(IEnumerable<User>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsersAsync()
        {
            List<User> users = null;

            try
            {
                users = await _userRepository.GetAllUsersAsync();
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while getting users list ", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error when adding users");
            }
            
            if(users == null || users.Count == 0)
            {
                return NotFound();
            }
            else
            {
                return Ok(users);
            }
        }

        [HttpGet(Name = "GetUserById")]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<User>> GetUserByIdAsync(int userId)
        {
            if(userId < 0) 
            {
                return BadRequest("User ID can not be less than 0");
            }
            User user;
            try
            {
                user = await _userRepository.GetAsync(userId);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while getting user with id: " + userId, ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error when getting user");
            }

            if(!UserValid(user))
            {
                return NotFound();
            }
            else
            {
                return Ok(user);
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<int>> AddUserAsync([FromBody] User newUser)
        {
            if(!UserValid(newUser))
            {
                return BadRequest("Given user is null or not valid");
            }
            try
            {
                int createdUserId = await _userRepository.CreateAsync(newUser);
                return CreatedAtAction(nameof(AddUserAsync), new { id = createdUserId });
            }
            catch(Exception ex) 
            {
                log.Error("Error occurred while adding a new user.", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error when adding user");
            }
        }

        [HttpPut]
        [ProducesResponseType(typeof(ActionResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateUserAsync([FromBody] User newUser)
        {
            if (!UserValid(newUser))
            {
                return BadRequest("Given user is null or not valid");
            }
            try
            {
                await _userRepository.UpdateAsync(newUser);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating a user.", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error when updating user");
            }

            return Ok();
            
        }

        [HttpDelete]
        [ProducesResponseType(typeof(ActionResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteUserAsync(int userId)
        {
            if (userId < 0)
            {
                return BadRequest("Given user is null or not valid");
            }
            try
            {
                await _userRepository.DeleteAsync(userId);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while deleting a user.", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error when deleting a user");
            }

            return Ok();

        }

        private bool UserValid(User user)
        {
            if (user == null)
            {
                return false;
            }

            if (string.IsNullOrEmpty(user.FirstName) || string.IsNullOrEmpty(user.LastName))
            {
                return false;
            }

            if (user.DateOfBirth == DateTime.MinValue || user.DateOfBirth > DateTime.Today)
            {
                return false;
            }

            return true;
        }
    }
}