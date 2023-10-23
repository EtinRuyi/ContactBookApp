using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ContactBookApp.UI.Models.UserDTO
{
    public class GetUserDto
    {
        public string Id { get; set; }
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }
        public string ImageUrl { get; set; }
    }
}
