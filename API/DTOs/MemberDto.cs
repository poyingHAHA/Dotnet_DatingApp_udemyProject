using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;

namespace API.DTOs
{
    public class MemberDto
    {
        public int Id { get; set; }

        public string Username { get; set; }
        // The age auto mapper is going to magically work this out for us based on the fact in our app user class we've got a property or a method inside here called GetAge.
        public int Age { get; set; }
        public string KnownAs { get; set; }
        public string PhotoUrl { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
        public string Gender { get; set; }
        public string Introduction { get; set; }
        public string LookingFor { get; set; }
        public string Interests { get; set; }
        public string Country { get; set; }
        // instead of using the photo and returning because this we know this is going to give us a problem.
        // What we'll do is we'll create a photoDto.
        public ICollection<PhotoDto> Photos { get; set; }
    }
}