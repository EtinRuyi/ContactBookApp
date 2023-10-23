using ContactBookApp.Model.Entity;
using ContactBookApp.Model.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ContactBookApp.Core.Services.Abstractions
{
    public interface IUserServices
    {
        public Task<IActionResult> CreateUserAsync (CreateNewUserViewModel model);
        public Task<IActionResult> DeleteUserAsync (string id);
        public Task<UserResponseModel> FindUserByIdAsync (string id);
        public Task<List<UserResponseModel>> GetAllUserAsync (int page, int pageSize);
        public Task<IActionResult> UpdateUserAsync(string id, UpdateViewModel model);
        public Task<List<UserResponseModel>> SearchUserAsync(string searchTerm);
    }
}
