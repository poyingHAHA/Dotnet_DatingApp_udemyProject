using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using API.Errors;

namespace API.Middleware
{
  // Now when we're using middleware or we're adding middleware into our DOT net API, we need to do certain things
  // And one of the things we need is a constructor of this.
  // So we're going to generate a constructor and what we need is something called a request delegate.
  // And the request delegate is what's next? What's coming up next in the middleware pipeline?
  // 
  public class ExceptionMiddleware
  {
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IHostEnvironment _env;

    public ExceptionMiddleware(
        RequestDelegate next,
        ILogger<ExceptionMiddleware> logger,
        IHostEnvironment env
    )
    {
      _next = next;
      _logger = logger;
      _env = env;
    }

    // When we add middleware, we have access to the actual HTTP request that's coming in.
    // So the first thing we're going to do is get our context and just simply pass this on to the next piece of middleware
    // Now, this piece of middleware is going to live at the very top Of our middleware and anything below this.
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch(Exception ex)
        {
            // And if we don't do this, then our exception is going to be silence in our terminal.
            // And that's not great because we do want to see as much information as possible about what's happened with any exception.
            _logger.LogError(ex, ex.Message);

            // What we'll do next is we're going to write out this exception to our response.
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            // And then what we're going to do is create a response.
            // And we're going to check to see what environment we're running in. Are we running in development mode?
            var response = _env.IsDevelopment() 
                ? new ApiException(context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString())
                : new ApiException(context.Response.StatusCode, "Internal Server Error");
            
            // And then what we'll do is we'll create some options because what we're going to do is send back this in JSON
            // Now by default, we want our JSON responses to go back in camel case.
            // So we're going to create some options to enable this because we need to serialize this response into a JSON response.
            var options = new JsonSerializerOptions{PropertyNamingPolicy = JsonNamingPolicy.CamelCase};
            
            var json = JsonSerializer.Serialize(response, options);
            
            await context.Response.WriteAsync(json);
            // And what we can do now is we can go back to our startup class and regardless of which mode we're running in, we're just going to use our middleware.

        }
    }
  }
}