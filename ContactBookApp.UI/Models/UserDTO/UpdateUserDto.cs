using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ContactBookApp.UI.Models.UserDTO
{
    public class UpdateUserDto
    {
        [Required]
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [Required]
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        [Required]
        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [Compare(nameof(Password))]
        [DataType(DataType.Password)]
        [DisplayName("Confirm Password")]
        public string ConfirmPassword { get; set; }
        [DisplayName("Upload Image")]
        public IFormFile UserImage { get; set; }
    }
}
