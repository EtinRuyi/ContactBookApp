using ContactBookApp.Commons.Security;
using ContactBookApp.Commons.Validations;
using ContactBookApp.Core.Services.Abstractions;
using ContactBookApp.Model.Entity;
using ContactBookApp.Model.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactBookApp.Core.Services.Implementations
{
    public class Authentication : IAuthentication
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserValidator _userValidator;
        public Authentication(UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration configuration, RoleManager<IdentityRole> roleManager, UserValidator userValidator)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _roleManager = roleManager;
            _userValidator = userValidator;
        }

        public async Task<SignInResult> CheckPasswordSignAsync(User user, string password, bool lockoutOnFailure)
        {
            return await _signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure);
        }

        public async Task<BaseResponse<LoginResponceViewModel>> Login(LoginRequestViewModel model)
        {
            var response = new BaseResponse<LoginResponceViewModel>();
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return response.Failed("User not found");
            }
            var validatePassword = await _signInManager.CheckPasswordSignInAsync(user, model.Password, lockoutOnFailure: false);
            if (!validatePassword.Succeeded)
            {
                return response.Failed("You are not authorized", StatusCodes.Status401Unauthorized);
            }
            string role;
            if (await _userManager.IsInRoleAsync(user, "Admin"))
            {
                role = "Admin";
            }
            else
            {
                role = "Regular";
            }
            var token = TokenGenerator.GenerateJwToken(user, role, _configuration);
            var result = new LoginResponceViewModel
            {
                Token = token,
                UserName = user.UserName,
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
            };
            return response.Success("logged in successfully", StatusCodes.Status200OK, result);
        }

        public async Task<BaseResponse<RegisterResponceViewModel>> RegisterAsync(RegisterViewModel model, string roleName, ModelStateDictionary modelState)
        {
            var user = new User
            {
                UserName = model.Email,
                Email = model.Email
            };

            var uniqueUser = await _userValidator.ValidateUserAsync(user, model.Password);
            var uniquePassword = await _userValidator.ValidateUserAsync(user,model.Password);
            var response = new BaseResponse<RegisterResponceViewModel>();
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!uniqueUser.Succeeded)
            {
                var errors = uniqueUser.Errors.Select(e => e.Description).ToList();
                return response.Failed("Email already exist", StatusCodes.Status400BadRequest);
            }
            if (!uniquePassword.Succeeded)
            {
                var errors = uniqueUser.Errors.Select(e => e.Description).ToList();
                return response.Failed("{Failed : PasswordRequiement not met}", StatusCodes.Status400BadRequest);
            }

            if (result.Succeeded)
            {
                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    await _roleManager.CreateAsync(new IdentityRole(roleName));
                }
                await _userManager.AddToRoleAsync(user, roleName);
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    modelState.AddModelError(string.Empty, error.Description);
                }
            }
            var status = new RegisterResponceViewModel
            {
                UserName = user.UserName,
            };
            return response.Success($"{status.UserName} created sucessfully", StatusCodes.Status200OK);
        }
    }
}
