using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using User_Project.Entities;
using User_Project.Services;

namespace User_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUserRepository _content;

        public UserController(IUserRepository content)
        {

            _content = content ?? throw new ArgumentNullException(nameof(content));
        }


        [HttpGet("all")]
        public async Task<IActionResult> GetUser(string username, string password)
        {
            var users = await _content.GetUserAsync();
            return Ok(users);
        }

        [HttpGet("{username}")]
        public async Task<IActionResult> GetUser(string username)
        {
            var user = await _content.GetUserByNameAsync(username);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost("AddTodo")]
        public async Task<ActionResult<User>> AddTodoItem(string username, string password, string email)
        {
            var todoItem = new User
            {
                Username = username,
                Email = email,
                Password = password,
            };

            await _content.AddUserAsync(todoItem);
            await _content.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new { id = todoItem.Id }, todoItem);
        }
    }
}
