using ContactBookApp.Model.Entity;
using ContactBookApp.Model.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactBookApp.Core.Services.Abstractions
{
    public interface IAuthentication
    {
        Task<BaseResponse<RegisterResponceViewModel>> RegisterAsync(RegisterViewModel model, string roleName, ModelStateDictionary modelState);
        Task<BaseResponse<LoginResponceViewModel>> Login(LoginRequestViewModel model);
        Task<SignInResult> CheckPasswordSignAsync(User user, string password, bool lockoutOnFailure);
    }
}
