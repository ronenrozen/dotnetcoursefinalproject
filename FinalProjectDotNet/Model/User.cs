using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProjectDotNet.Model
{
    public class User
    {
        [Required(ErrorMessage = "Email is mandatory")]
        public string Email { get; set; }
        public string password { get; set; }

    }
}
