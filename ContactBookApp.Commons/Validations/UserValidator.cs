using ContactBookApp.Model.Entity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactBookApp.Commons.Validations
{
    public class UserValidator
    {
        private readonly UserManager<User> _userManager;

        public UserValidator(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IdentityResult> ValidateUserAsync(User user, string password)
        {
            var usernameExists = await _userManager.FindByNameAsync(user.UserName);
            if (usernameExists != null)
            {
                return IdentityResult.Failed(new IdentityError { Code = "DuplicateUserName", Description = "UserName already exists" });
            }

            var emailExists = await _userManager.FindByEmailAsync(user.Email);
            if (emailExists != null)
            {
                return IdentityResult.Failed(new IdentityError { Code = "DuplicateEmail", Description = "Email already exists" });
            }

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> ValidateUserPasswordAsync(User user, string password)
        {
            var result = await _userManager.PasswordValidators.First().ValidateAsync(_userManager, user, password);
            if (!result.Succeeded)
            {
                return result;
            }

            return IdentityResult.Success;
        }

        //public async Task<IdentityResult> ValidateUserAsync(Contact user)
        //{
        //    var emailExists = await _userManager.FindByEmailAsync(user.Email);
        //    if (emailExists != null)
        //    {
        //        return IdentityResult.Failed(new IdentityError { Code = "DuplicateEmail", Description = "Email already exists" });
        //    }
        //    return IdentityResult.Success;
        //}

        public async Task<IdentityResult> ValidateUserAsync(User user)
        {
            var userValidator = new UserValidator<User>();
            var validationResult = await userValidator.ValidateAsync(_userManager, user);
            return validationResult;
        }
    }
}
