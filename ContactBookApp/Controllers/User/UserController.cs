using ContactBookApp.Core.Services.Abstractions;
using ContactBookApp.Model.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ContactBookApp.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserServices _userServices;
        public UserController(IUserServices userServices)
        {
            _userServices = userServices;
        }

        [HttpGet("Add-new-user")]
        public async Task<IActionResult> AddNewUser([FromBody] PostNewUserViewModel model)
        {
            return await _userServices.CreateUserAsync(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            return await _userServices.DeleteUserAsync(id);
        }

        [HttpGet("Get/{id}")]
        public async Task<IActionResult> GetUserById (string id)
        {
            return await _userServices.FindUserByIdAsync(id);
        }

        [Authorize(Roles = "Regular")]
        [HttpPut("Udate/{id}")]
        public async Task<IActionResult> UdateUserById(string id, [FromBody] PutViewModel model)
        {
            var user = await _userServices.FindUserByIdAsync (id);
            if (user == null)
            {
                return NotFound(new {Message = "User not found"});
            }
            return await _userServices.UpdateUserAsync(id, model);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("Get-All-Users")]
        public async Task<IActionResult> GetAllUser(int page = 1, int pageSize = 10)
        {
            return await _userServices.GetAllUserAsync(page, pageSize);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("Search-User")]
        public async Task<IActionResult> SearchUsers (string searchTearm)
        {
            if (string.IsNullOrEmpty(searchTearm))
            {
                return BadRequest("Enter UserName or Email or Id or Phone number");
            }
            var users = await _userServices.SearchUserAsync(searchTearm);
            return Ok(users);
        }
    }
}
