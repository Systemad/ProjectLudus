using Microsoft.EntityFrameworkCore;
using WebAPI.Features.Common.Users.Models;

namespace WebAPI.Features.DataAccess;

public class LudusContext(DbContextOptions<LudusContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<UserImage> UserImages { get; set; }
    public DbSet<GameWishlist> Wishlists { get; set; }
    public DbSet<GameHype> Hypes { get; set; }
    public DbSet<GameList> Lists { get; set; }
    public DbSet<GameListItem> ListItems { get; set; }
}
