using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
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
            .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => src.Photos.FirstOrDefault(x => x.IsMain).Url));
        // What we also need is a map from our photo DTO or from our photo entity to our photo DTO, and that's
        CreateMap<Photo, PhotoDto>();
        // What we also need to do, because we're going to be adding this as a dependency that we can inject,
        // we need to add this to our application service extensions.
    }
  }
}