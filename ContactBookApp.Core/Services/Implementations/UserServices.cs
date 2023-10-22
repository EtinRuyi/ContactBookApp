using ContactBookApp.Commons.Validations;
using ContactBookApp.Core.Services.Abstractions;
using ContactBookApp.Model.Entity;
using ContactBookApp.Model.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContactBookApp.Core.Services.Implementations
{
    public class UserServices : IUserServices
    {
        private readonly UserManager<User> _userManager;
        private readonly UserValidator _userValidator;
        public UserServices(UserManager<User> userManager, UserValidator userValidator)
        {
            _userManager = userManager;
            _userValidator = userValidator;
        }

        public async Task<IActionResult> CreateUserAsync(PostNewUserViewModel model)
        {
            var user = new User 
            { 
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.Email, 
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
            };

            var validationResult = await _userValidator.ValidateUserAsync(user, model.Password);
            if (!validationResult.Succeeded)
            {
                var errors = validationResult.Errors.Select(e => e.Description).ToList();
                return new BadRequestObjectResult(new {Message = "Failed to create user", Erroes = errors});
            }

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                return new OkObjectResult(new { Message = "User Created Successfully" });
            }
            else
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                return new BadRequestObjectResult(new { Message = "Failed to create user", Erros = errors });
            }

        }

        public async Task<IActionResult> DeleteUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return new NotFoundObjectResult(new { Message = "User not found." });
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                return new BadRequestObjectResult(new { Message = "Failed to delete user." });
            }
            return new OkObjectResult(new {Message = "User deleted successfully" });
        }

        public async Task<IActionResult> FindUserByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return new NotFoundObjectResult( new { Message = "User not found"});
            }
            return new OkObjectResult(user);
        }

        public async Task<IActionResult> GetAllUserAsync(int page, int pageSize)
        {
            var allUsers = await _userManager.Users.CountAsync();
            var totalPages = (int)Math.Ceiling(allUsers / (double)pageSize);
            page = Math.Max(1, Math.Min(totalPages, page));

            var users = await _userManager.Users
                .OrderBy(x => x.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var paginatedResult = new PaginatedUserViewModel
            {
                TotalUsers = users.Count,
                CurentPage = page,
                PadeSize = pageSize,
                Users = users,
            };

            return new OkObjectResult(paginatedResult);
        }

        //public async Task<List<User>> SearchUserAsync(string searchTerm)
        //{
        //    try
        //    {
        //        var result = await _userManager.Users
        //               .Where(u => u.UserName.Contains(searchTerm) || u.Email.Contains(searchTerm) || u.Id.Contains(searchTerm)
        //               || u.PhoneNumber.Contains(searchTerm)).ToListAsync();

        //        return result;
        //    }
        //    catch (Exception ex)
        //    {

        //        throw new ApplicationException(ex.Message);
        //    }
        //}

        public async Task<IActionResult> SearchUserAsync(string searchTerm)
        {
            try
            {
                var result = await _userManager.Users
                    .Where(u => u.UserName.Contains(searchTerm) || u.Email.Contains(searchTerm) || u.Id.Contains(searchTerm)
                    || u.PhoneNumber.Contains(searchTerm)).ToListAsync();

                if (result.Count == 0)
                {
                    // No users found, return a "User not found" message
                    return new NotFoundObjectResult(new { Message = "User not found" });
                }

                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                // Handle the exception or return an error response
                return new BadRequestObjectResult(new { Message = "An error occurred while searching for users" });
            }
        }

        public async Task<IActionResult> UpdateUserAsync(string id, PutViewModel model)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return new NotFoundObjectResult(new { Message = "User not fount" });
            }
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.PhoneNumber = model.PhoneNumber;

            var validationResult = await _userValidator.ValidateUserAsync(user);
            if (!validationResult.Succeeded)
            {
               var errors = validationResult.Errors.Select(e => e.Description).ToList();
                return new BadRequestObjectResult(new { Message = "Validation Failed", Errors = errors});
            }
            if (!string.IsNullOrWhiteSpace(model.Password))
            {
                var passwordChange = await _userManager.ChangePasswordAsync(user, model.Password, model.Password);
                if (!passwordChange.Succeeded)
                {
                    return new BadRequestObjectResult(new {Message = "Failed to change password", passwordChange.Errors});
                }
            }

            var updatedResult = await _userManager.UpdateAsync(user);
            if (!updatedResult.Succeeded)
            {
                return new BadRequestObjectResult(new {Message = "Failed to update user", updatedResult.Errors});
            }
            return new OkObjectResult(new {Message = "User updated sucessfully"});

        }
    }
}
