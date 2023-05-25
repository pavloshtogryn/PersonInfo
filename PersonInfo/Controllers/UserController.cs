using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonInfo.Models;

namespace PersonInfo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly PersonInfoDbContext _context;

        public UserController(PersonInfoDbContext context)
        {
            _context = context;
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
                users = await _context.Users.ToListAsync();
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
                user = await _context.Users.FindAsync(userId);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while getting user with id: " + userId, ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error when getting user");
            }

            if(user == null || !UserValid(user))
            {
                return NotFound();
            }
            else
            {
                return Ok(user);
            }
        }

        [HttpPost ("add")]
        [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<int>> AddUserAsync([FromBody] User newUser)
        {
            if(newUser == null || !UserValid(newUser))
            {
                return BadRequest("Given user is null");
            }
            try
            {
                await _context.Users.AddAsync(newUser);
                var result = await _context.SaveChangesAsync();

                if (result > 0)
                {
                    return CreatedAtAction(nameof(AddUserAsync), new { id = newUser.Id });
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
            }
            catch(Exception ex) 
            {
                log.Error("Error occurred while adding a new user.", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error when adding user");
            }
        }
        
        [HttpPost("edit")]
        [ProducesResponseType(typeof(ActionResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateUserAsync([FromBody] User user)
        {
            if (user == null || !UserValid(user))
            {
                return BadRequest("Given user is null");
            }
            try
            {
                var userToEdit = _context.Users.Where(u => u.Id == user.Id).FirstOrDefault();
                if (userToEdit == null) 
                {
                    return NotFound();
                }
                else
                {
                    userToEdit.FirstName = user.FirstName;
                    userToEdit.LastName = user.LastName;
                    userToEdit.DateOfBirth = user.DateOfBirth;
                }

                var result = await _context.SaveChangesAsync();

                if (result > 0)
                {
                    return Ok();
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }

            }
            catch (Exception ex)
            {
                log.Error("Error occurred while e a new user.", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error when adding user");
            }
        }

        private bool UserValid(User user)
        {
            return !String.IsNullOrEmpty(user.FirstName) && !String.IsNullOrEmpty(user.LastName) && user.DateOfBirth != DateTime.MinValue;
        }
    }
}