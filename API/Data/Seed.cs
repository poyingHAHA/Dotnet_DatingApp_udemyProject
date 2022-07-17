using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class Seed
    {
        // Where do we call this method from?
        // Well, the current recommended place is in the program.cs class, and this does change periodically.
        // Microsoft changed their recommendations, but where we start our application is a fine place to seed data because we're going to do this inside the main method.
        // And what goes on inside Main in Program.cs happens before our application is actually started.
        public static async Task SeedUsers(DataContext context)
        {
            // to check if user exists
            if(await context.Users.AnyAsync()) return;

            var userData = await System.IO.File.ReadAllTextAsync("Data/UserSeedData.json");
            // And there's two options here, but we're going to use system text JSON.
            // This has been available since dot net three and we no longer need to use Newton soft JSON, which has been around for many years.
            // But Microsoft have finally created a version of this that we can use natively inside .Net core.
            var users = JsonSerializer.Deserialize<List<AppUser>>(userData);
            foreach(var user in users)
            {
                using var hmac = new HMACSHA512();
                user.UserName = user.UserName.ToLower();
                // using password as password for seed data
                user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("password"));
                user.PasswordSalt = hmac.Key;

                // And we don't need to await this method because when we're adding, all we're doing, don't forget, is tracking, adding tracking to the user through entity framework. 
                // We're not doing anything with a database at this point.
                context.Users.Add(user);
            }

            await context.SaveChangesAsync();
            
        }
    }
}