using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Helpers;
using API.Repositories;
using API.Repositories.Impl;
using API.Services;
using API.Services.Impl;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(
            this IServiceCollection services, 
            IConfiguration config
        )
        {
            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));
            services.AddScoped<IPhotoService, PhotoService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<ILikesRepository, LikesRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<LogUserActivity>();
            // Now we only have a single project, so we only have a single assembly of where these can be created.
            // And this is enough for auto mapper to go ahead and find those profiles, the create maps that we created inside this class, and that's the configuration set up for auto mapper.
            services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
            services.AddDbContext<DataContext>(options => {
                options.UseSqlite(config.GetConnectionString("DefaultConnection"));
            });

            return services;
        }
    }
}