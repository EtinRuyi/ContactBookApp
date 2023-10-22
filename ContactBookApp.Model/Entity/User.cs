using Microsoft.AspNetCore.Identity;

namespace ContactBookApp.Model.Entity
{
    public class User : IdentityUser
    {
        public string ImageUrl { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<Contact> Contacts { get; set; }
        //public int ContactCount { get; set; }
    }
}
