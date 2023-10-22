using ContactBookApp.Core.Services.Abstractions;
using ContactBookApp.Model.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ContactBookApp.Controllers.Contact
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly IContactServices _contactServices;
        public ContactsController(IContactServices contactServices)
        {
            _contactServices = contactServices;
        }

        //[Authorize]
        [HttpPost("Add-Contact")]
        public async Task<IActionResult> AddContact([FromBody] ContactViewModel model, string userId)
        {
            return await _contactServices.CreateContactAsync(model, userId);
        }

        [HttpGet("Get-Contact/{id}")]
        public async Task<BaseResponse<ContactResponseModel>> GetContactById(string id)
        {
            return await _contactServices.FindContactByIdAsync(id);
        }

        [HttpDelete("Delete-Contact/{id}")]
        public async Task<IActionResult> DeleteContactById (string id)
        {
            return await _contactServices.DeleteContactAsync(id);
        }

    }
}
