using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactBookApp.Model.ViewModels
{
    public class ContactViewModel
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Emial { get; set; }
        [Required]
        public string PhoneNmuber { get; set; }
        [Required]
        public string Address { get; set; }
    }
}
