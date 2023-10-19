using ContactBookApp.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactBookApp.Model.ViewModels
{
    public class PaginatedUserViewModel
    {
        public int TotalUsers { get; set;}
        public int PadeSize { get; set;}
        public int CurentPage { get; set;}
        public List<User> Users { get; set;}
    }
}
