using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
  public class BuggyController : BaseApiController
  {
    private readonly DataContext _context;

    public BuggyController(DataContext context)
    {
      _context = context;
    }

    // The purpose of this is to test our 401 unauthorized responses.
    [Authorize]
    [HttpGet("auth")]
    public ActionResult<string> GetSecret()
    {
       return "secret text"; 
    }

    [HttpGet("not-found")]
    public ActionResult<AppUser> GetNotFound()
    {
        // we'll look for something that we know for sure is not going to exist.
        var thing = _context.Users.Find(-1); 

        // And then what we'll do, we'll check the status of thing and if thing is equal to null.
        if(thing == null) return NotFound();

        // And if we do somehow manage to find this, we'll return whatever the thing actually is.
        return Ok(thing);
    }   

    [HttpGet("server-error")]
    public ActionResult<string> GetServerError()
    {
       // And in this case, once again, we'll attempt to find something or we'll create a variable called thing.
       var thing = _context.Users.Find(-1);

       // But what we want to do here is generate an exception from this particular method.
       // So what we'll say is we'll add another variable and we'll say, this is the thing to return.
       // And in this case, what we'll try and do is set the thing to a string.
       // And when we try and execute a method on null such as this, then what we're going to generate from this is a null reference exception.
       var thingToReturn = thing.ToString();

       // So we'll just use this as an example of a server exception.
       return thingToReturn;
    }

    [HttpGet("bad-request")]
    public ActionResult<string> GetBadRequest()
    {
       return BadRequest("This was not a good request."); 
    }
  }
}