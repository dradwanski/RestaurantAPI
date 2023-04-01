using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantAPI.Models
{
    public class RegisterUserDto
    {
        public string email { get; set; }
        public string password { get; set; }
        public string confirmpassword { get; set; }
        public DateTime? dateOfBirth { get; set; }
        public string nationality { get; set; }
        public int roleId { get; set; } = 1;
    }
}
