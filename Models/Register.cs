using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WarrantyRegistrationApp.Models
{
    public class Register
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Email { get; set; }
    }
}
