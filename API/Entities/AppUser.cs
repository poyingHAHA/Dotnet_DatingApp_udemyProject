using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Extensions;

namespace API.Entities
{
    public class AppUser
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string KnownAs { get; set; }
        public DateTime CreatedBy { get; set; } = DateTime.Now;
        public DateTime LastActive { get; set; } = DateTime.Now;
        public string Gender { get; set; }
        public string Introduction { get; set; }
        public string LookingFor { get; set; }
        public string Interests { get; set; }
        public string Country { get; set; }
        public ICollection<Photo> Photos { get; set; }
        
        // In order for Mapper to go in, retrieve this, it's got to go and get the appUser.
        // So we're kind of defeating the purpose of what we're doing here by doing it this way.
        // So what we're going to do is not use this getAge methods to go and get the age.
        // because it's not particularly efficient.
        // public int GetAge()
        // {
        //     return DateOfBirth.CalculateAge();
        // }
        // But what we'll do is we'll go to our automatic profile and we'll add another property and say ForMember
    }
}