using ContactBookApp.Model.Entity;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactBookApp.Core.Services.Abstractions
{
    public interface ICloudinaryService
    {
        Task<string> UploadImageAsync(User user, IFormFile image);
    }
}
