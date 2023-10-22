using ContactBookApp.Model.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ContactBookApp.Core.Services.Abstractions
{
    public interface IUserServices
    {
        public Task<IActionResult> CreateUserAsync (PostNewUserViewModel model);
        public Task<IActionResult> DeleteUserAsync (string id);
        public Task<IActionResult> FindUserByIdAsync (string id);
        public Task<IActionResult> UpdateUserAsync (string id, PutViewModel model);
        public Task<IActionResult> GetAllUserAsync (int page, int pageSize);
        public Task<IActionResult> SearchUserAsync(string searchTerm);
    }
}
