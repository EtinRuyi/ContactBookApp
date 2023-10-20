using ContactBookApp.Core.Services.Abstractions;
using ContactBookApp.Data;
using ContactBookApp.Model.Entity;
using ContactBookApp.Model.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactBookApp.Core.Services.Implementations
{
    public class ContactServices : IContactServices
    {
        private readonly ContactBookAppDbContext _dbcontext;
        private readonly UserManager<User> _userManager;
        public ContactServices(ContactBookAppDbContext dbcontext, UserManager<User> userManager)
        {
            _dbcontext = dbcontext;
            _userManager = userManager;
        }

        public async Task<IActionResult> CreateContactAsync(ContactViewModel model, string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return new BadRequestObjectResult(new { Message = "Id cannot be empty" });
            }

            var idExists = await _userManager.FindByEmailAsync(id);
            if (idExists == null)
            {
                return new BadRequestObjectResult(new { Message = "Invalid Id" });
            }
            var contact = new Contact
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Emial,
                Address = model.Address,
                CreatedAt = DateTime.UtcNow,
                UpdatedAp = DateTime.UtcNow,
                IsDepreciated = false,
                UserId = id,
            };

            var uniqueUser = await _dbcontext.Contacts.FirstOrDefaultAsync(x => x.Email == model.Emial);
            if (uniqueUser != null)
            {
                return new BadRequestObjectResult(new { Message = "Email aleady exists" });
            }
            await _dbcontext.Contacts.AddAsync(contact);
            await _dbcontext.SaveChangesAsync();
            return new OkObjectResult(new { Message = "User contact added successfully" });
        }

        public async Task<IActionResult> DeleteContactAsync(string id)
        {
            var user = await _dbcontext.Contacts.FindAsync(id);
            if (user == null)
            {
                return new NotFoundObjectResult(new {Message = "user not foune"});
            }
            _dbcontext.Contacts.Remove(user);
            await _dbcontext.SaveChangesAsync();
            return new OkObjectResult(new { Message = "Contact deleted successfully" });
        }

        public Task<BaseResponse<ContactResponseModel>> FindContactByIdAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
}
