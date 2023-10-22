using ContactBookApp.Model.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ContactBookApp.Core.Services.Abstractions
{
    public interface IContactServices
    {
        Task<IActionResult> DeleteContactAsync(string id);
        Task<IActionResult> CreateContactAsync(ContactViewModel model, string id);
        Task<BaseResponse<ContactResponseModel>> FindContactByIdAsync(string id);
    }
}
