using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class RegisterDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        // And we're just adding an additional validator here so that we'll see additional responses when we hit this particular method.
        [StringLength(8, MinimumLength = 4)] 
        public string Password { get; set; }
    }
}