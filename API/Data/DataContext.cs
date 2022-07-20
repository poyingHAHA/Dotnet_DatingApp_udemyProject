using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
  public class DataContext : DbContext
  {
    public DataContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<AppUser> Users { get; set; }

    public DbSet<UserLike> Likes { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
      base.OnModelCreating(builder);

      // we pass in the entity as a type parameter of what we want to configure.
      builder.Entity<UserLike>()
              .HasKey(k => new {k.SourceUserId, k.LikedUserId});// we're going to configure this key ourselves and it's going to be a combination of the source user ID and the liked user ID.

      builder.Entity<UserLike>()
            .HasOne(s => s.SourceUser)
            .WithMany(l => l.LikedUsers)
            .HasForeignKey(s => s.SourceUserId)
            .OnDelete(DeleteBehavior.Cascade);

      builder.Entity<UserLike>()
            .HasOne(s => s.LikedUser)
            .WithMany(l => l.LikedByUsers)
            .HasForeignKey(s => s.LikedUserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
  }
}