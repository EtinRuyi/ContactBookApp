using ContactBookApp.Model.Entity;
using ContactBookApp.Model.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ContactBookApp.Core.Services.Abstractions;

namespace ContactBookApp.Authentications
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthentication _authenticationService;
        private readonly ILogger<AuthenticationController> _logger;
        private readonly UserManager<User> _userManager;

        public AuthenticationController(IAuthentication authenticationService, ILogger<AuthenticationController> logger, UserManager<User> userManager)
        {
            _authenticationService = authenticationService;
            _logger = logger;
            _userManager = userManager;
        }

        [HttpPost("Register-User")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _authenticationService.RegisterAsync(model, "Regular", ModelState);
            if (result != null)
            {
               return Ok(result);
            }
            else 
            { 
                return BadRequest(new {Message ="Failed to create Regular user"}); 
            }
        }

        [HttpPost("Register-Admin")]
        public async Task<IActionResult> CreateAdmin([FromBody] RegisterViewModel model)
        {
            var result = await _authenticationService.RegisterAsync(model, "Admin", ModelState);
            if (result != null)
            {
                return Ok(result);
            }
            else { return BadRequest(new { Message = "Failed to create admin user" }); }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _authenticationService.Login(model);
            return Ok(response);
        }
    }
}
