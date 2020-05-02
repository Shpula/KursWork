using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebClient.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Specialication { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public bool Block { get; set; }
    }
}
