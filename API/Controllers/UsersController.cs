using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Repositories;
using API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
  [Authorize] 
  public class UsersController : BaseApiController
  {
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IPhotoService _photoService;

    public UsersController(
      IUserRepository userRepository, 
      IMapper mapper,
      IPhotoService photoService
    )
    {
      _photoService = photoService;
      _userRepository = userRepository;
      _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
    {
      var users = await _userRepository.GetMembersAsync();
      
      return Ok(users);
    }

    [HttpGet("{username}", Name = "GetUser")]
    public async Task<ActionResult<MemberDto>> GetMembersAsync(string username)
    {
      return await _userRepository.GetMemberAsync(username);
    }

    [HttpPut]
    public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
    {
      // Now what this should give us is the user's username from the token that the API uses to authenticate this user.
      var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

      _mapper.Map(memberUpdateDto, user);
      _userRepository.Update(user);

      if(await _userRepository.SaveAllSync()) return NoContent();

      return BadRequest("Failed to update user");
    } 

    [HttpPost("add-photo")]
    public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
    {
      var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());
      var result = await _photoService.AddPhotoAsync(file);

      if(result.Error != null) return BadRequest(result.Error.Message);

      var photo = new Photo
      {
        Url = result.SecureUrl.AbsoluteUri,
        PublicId = result.PublicId
      };

      // If it is, then we know that this is the first image that the users are uploading 
      // and if it is the first photo uploaded, then we're going to set this one to Main.
      if(user.Photos.Count == 0)
      {
        photo.IsMain = true;
      }

      user.Photos.Add(photo);

      if(await _userRepository.SaveAllSync())
      {
        // return _mapper.Map<PhotoDto>(photo);
        return CreatedAtRoute("GetUser", new {username = user.UserName}, _mapper.Map<PhotoDto>(photo));
      }

      return BadRequest("Problem Adding Photo");
    }

    [HttpPut("set-main-photo/{photoId}")]
    public async Task<ActionResult> SetMainPhoto(int photoId)
    {
      // We can trust the information inside the token. 
      // There's no trickery going on there because our servers signed the token.
      var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

      // this is not asynchronous because we've already got the user in memory at this point, so we're not go to database now.
      // 
      var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

      if(photo.IsMain) return BadRequest("This is already your main photo");

      var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);

      if(currentMain != null) currentMain.IsMain = false;
      photo.IsMain = true;

      if(await _userRepository.SaveAllSync()) return NoContent();

      return BadRequest("Fail to set main photo");
    }
  }
}