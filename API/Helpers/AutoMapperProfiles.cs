using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Extensions;
using AutoMapper;

namespace API.Helpers
{
  // Now the idea of auto mapper is it helps us map from one object to another.
  // 
  public class AutoMapperProfiles : Profile
  {
    public AutoMapperProfiles()
    {
        // Now what we specify in here is where we want to map from and where we want to map to.
        CreateMap<AppUser, MemberDto>()
            .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => src.Photos.FirstOrDefault(x => x.IsMain).Url))
            .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()));
            // So this is our new mapping configuration and let's see if this has made any difference.
            // So we head back to Postman once again. We'll just send this request again and we'll go back to our terminal and scroll down to the bottom and
            // take a look at our query.
            // So what we're doing now is we've made our query a bit more efficient. We're not getting all of the properties. I can't promise you we've made too much difference by doing what we're doing here, but it is an improvement.
        CreateMap<Photo, PhotoDto>();
        CreateMap<MemberUpdateDto, AppUser>();
    }
  }
}