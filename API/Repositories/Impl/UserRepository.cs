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
  public class UserRepository : IUserRepository
  {
    private readonly DataContext _context;

    public UserRepository(DataContext context)
    {
      _context = context;
    }

    public async Task<AppUser> GetUserByIdAsync(int id)
    {
      return await _context.Users.FindAsync(id);
    }

    public async Task<AppUser> GetUserByUsernameAsync(string username)
    {
      return await _context.Users // get our user from our database
        .Include(p => p.Photos) // include the photos cllection
        .SingleOrDefaultAsync(x => x.UserName == username); // pass it back to userController
    }

    public async Task<IEnumerable<AppUser>> GetUsersAsync()
    {
        return await _context.Users
            .Include(p => p.Photos)
            .ToListAsync();
    }

    public async Task<bool> SaveAllSync()
    {
        return await _context.SaveChangesAsync() > 0;
    }

    public void Update(AppUser user)
    {
        _context.Entry(user).State = EntityState.Modified;
    }

    public async Task<MemberDto> GetMemberAsync(string username)
    {
      // Supposing we didn't want to use auto mapper
      return await _context.Users
            .Where(x => x.UserName == username)
            .Select(user => new MemberDto
            {
              // inside here we would start manually mapping all the properties.
              // Now, in this select statement, what we would do is we would then start manually mapping the properties
              // that we need to select from our database that we're going to put inside and return for our member data.
              Id = user.Id,
              Username = user.UserName
              // Now we've got about 20 properties in here, so I'm not going to go through the whole thing and we don't need to go through the whole thing because auto mapper helps us out here.
              // Auto Mapper gives us the equivalent of doing this for every single property and it allows us to project
              // inside our repository and it's only going to select the properties that we actually need.
            }).SingleOrDefaultAsync(); // So what we'll do is we'll say SingleOrDefaultAsync, and then this goes to our database. This is where we execute the query.
    }

    public Task<IEnumerable<MemberDto>> GetMembersAsync()
    {
      throw new NotImplementedException();
    }
    
  }
}