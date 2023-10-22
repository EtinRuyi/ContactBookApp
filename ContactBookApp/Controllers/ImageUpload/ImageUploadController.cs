using ContactBookApp.Core.Services.Abstractions;
using ContactBookApp.Model.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ContactBookApp.ImageUpload
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageUploadController : ControllerBase
    {
        private readonly ICloudinaryService _cloudinaryService;
        private readonly UserManager<User> _userManager;
        public ImageUploadController(ICloudinaryService cloudinaryService, UserManager<User> userManager)
        {
            _cloudinaryService = cloudinaryService;
            _userManager = userManager;
        }

        //[Authorize]
        [HttpPatch("Upload-Image/{id}")]
        public async Task<IActionResult> UploadUserImage(string id, IFormFile image)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound(new {Message = "User not found"});
            }
            if (image == null)
            {
                return BadRequest(new { Message = "Image file is required" });
            }
            if (image.Length <=0)
            {
                return BadRequest(new { Message = "Image path is empty" });
            }
            var imageUrl = await _cloudinaryService.UploadImageAsync(user, image);
            if (imageUrl == null)
            {
                return BadRequest(new { Message = "Failed to upload image" });
            }
            var updatedResult = await _userManager.UpdateAsync(user);
            if (!updatedResult.Succeeded)
            {
                return BadRequest(new { Message = "Failed to uptade new image" });
            }
            return Ok(new {Message = "Image uploaded successfully", ImageUrl = imageUrl});
        }



    }
}
