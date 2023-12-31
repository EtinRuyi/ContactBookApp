﻿using ContactBookApp.Core.Services.Abstractions;
using ContactBookApp.Data;
using ContactBookApp.Model.Entity;
using ContactBookApp.Model.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

            var idExists = await _userManager.FindByIdAsync(id);
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
                PhoneNumber = model.PhoneNmuber, 
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

        public async Task<BaseResponse<ContactResponseModel>> FindContactByIdAsync(string id)
        {
            var response =  new BaseResponse<ContactResponseModel>();
            var user = await _dbcontext.Contacts.FindAsync(id);
            if (user == null)
            {
                return response.Failed("User not found", StatusCodes.Status404NotFound);
            }
            var status = new ContactResponseModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
            };
            return response.Success("Success", StatusCodes.Status200OK, status);
        }
    }
}
