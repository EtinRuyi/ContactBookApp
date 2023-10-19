using ContactBookApp.Core.Services.Abstractions;
using ContactBookApp.Model.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactBookApp.Core.Services.Implementations
{
    public class UserServices : IUserServices
    {
        public Task<IActionResult> CreateUserAsync(PostNewUserViewModel model)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> DeleteUserAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> FindUserByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> GetAllUserAsync(int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> SearchUserAsync(string searchTerm)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> UpdateUserAsync(string id, PutViewModel model)
        {
            throw new NotImplementedException();
        }
    }
}
