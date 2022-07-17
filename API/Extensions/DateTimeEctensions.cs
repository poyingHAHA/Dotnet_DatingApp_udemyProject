using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Extensions
{
    public static class DateTimeEctensions
    {
        public static int CalculateAge(this DateTime datetime)
        {
            var today = DateTime.Today;
            var age = today.Year - datetime.Year;
            if(datetime.Date > today.AddYears(-age)) age--;
            return age;
        }
    }
}