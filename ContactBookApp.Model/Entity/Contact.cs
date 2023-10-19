using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactBookApp.Model.Entity
{
    public class Contact : BaseEntity
    {
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        [ForeignKey("UserId")]
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
