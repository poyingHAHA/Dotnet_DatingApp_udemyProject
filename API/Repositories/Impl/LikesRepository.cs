using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories.Impl
{
  public class LikesRepository : ILikesRepository
  {
    private readonly DataContext _context;
    public LikesRepository(DataContext context)
    {
        _context = context;
    }

    public Task<UserLike> GetUserLike(int sourceId, int likedUserId)
    {
      throw new NotImplementedException();
    }

    public Task<IEnumerable<LikeDto>> GetUserLikes(string predicate, int userId)
    {
      throw new NotImplementedException();
    }

    public Task<AppUser> GetUserWithLikes(int userId)
    {
      throw new NotImplementedException();
    }
  }
}