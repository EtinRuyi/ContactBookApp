using ContactBookApp.Model.Entity;
using ContactBookApp.Model.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ContactBookApp.Core.Services.Abstractions
{
    public interface IAuthentication
    {
        Task<BaseResponse<RegisterResponceViewModel>> RegisterAsync(RegisterViewModel model, string roleName, ModelStateDictionary modelState);
        Task<BaseResponse<LoginResponceViewModel>> Login(LoginRequestViewModel model);
        Task<SignInResult> CheckPasswordSignAsync(User user, string password, bool lockoutOnFailure);
    }
}
