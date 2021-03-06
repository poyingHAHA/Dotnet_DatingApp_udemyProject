using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Helpers;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories.Impl
{
  public class UserRepository : IUserRepository
  {
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public UserRepository(DataContext context, IMapper mapper)
    {
      _context = context;
      _mapper = mapper;
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
            .ProjectTo<MemberDto>(_mapper.ConfigurationProvider) //And then what we can do is say mapper and we can pass in the configuration provider so it can go and get the configuration that we provided in our auto mapper profiles here and then what we can do, because now we've completed this functionality and now what we can do is go and see if we've made any improvements.
            // We're going to see if this particular tactic of projecting makes us more efficient in our database query.
            // So what we'll do is we'll go back to our users controller and where we're getting the user. Instead of saying get user by username, we're going to say GetMemberAsync andWe don't need to map inside our controller..
            .SingleOrDefaultAsync(); // So what we'll do is we'll say SingleOrDefaultAsync, and then this goes to our database. This is where we execute the query.
    }

    public async Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams)
    {
      var query = _context.Users.AsQueryable();

      query = query.Where(u => u.UserName != userParams.CurrentUsername);
      query = query.Where(u => u.Gender == userParams.Gender);

      var minDob = DateTime.Today.AddYears(-userParams.MaxAge - 1);
      var maxDob = DateTime.Today.AddYears(-userParams.MinAge);

      query = query.Where(u => u.DateOfBirth >= minDob && u.DateOfBirth <= maxDob);
      query = userParams.OrderBy switch
      {
        "created" => query.OrderByDescending(u => u.CreatedBy),
        // And the way that we specify default in this switch statement is an underscore  
        _ => query.OrderByDescending(u => u.LastActive)
      };

      return await PagedList<MemberDto>.CreateAsync(
        query.ProjectTo<MemberDto>(_mapper.ConfigurationProvider).AsNoTracking(),
        userParams.PageNumber, userParams.PageSize
      );
    }
  }
}