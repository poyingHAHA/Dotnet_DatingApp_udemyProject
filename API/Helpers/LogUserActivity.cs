using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Extensions;
using API.Repositories;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Helpers
{
  public class LogUserActivity : IAsyncActionFilter
  {
    // So what we have is the context of the action that is executing.
    // And what we also have is inside this method as well is we have next what's going to happen next after
    // the action is executed and we can use this property. Or this parameter.
    // To execute the action and then do something after this is executed.
    // So what we'll do is we want to get access to the context after this is executed.
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
      // And if we take a look at what we get inside there, then this gives us access to our HTTP context, the model state, the results, root data and our controller.
      var resultContext = await next();

      // Now if the user sent up a token and we've authenticated the user, then this is going to be true.
      if(!resultContext.HttpContext.User.Identity.IsAuthenticated) return;

      var username = resultContext.HttpContext.User.GetUsername();
      var repo = resultContext.HttpContext.RequestServices.GetService<IUserRepository>();
      var user = await repo.GetUserByUsernameAsync(username);
      user.LastActive = DateTime.Now;
      await repo.SaveAllSync();
    }
  }
}