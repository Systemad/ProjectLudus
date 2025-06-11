using Ludus.Server.Features.Common.Users.Models;
using Microsoft.EntityFrameworkCore;

namespace Ludus.Server.Features.DataAccess;

public class LudusContext(DbContextOptions<LudusContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<UserImage> UserImages { get; set; }

    //public DbSet<UserImage> Posts { get; set; }
    public DbSet<GameWishlist> Wishlists { get; set; }
    public DbSet<GameHype> GameHypes { get; set; }
}
