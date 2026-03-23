using Microsoft.EntityFrameworkCore;
using PlayAPI.Data;

namespace PlayAPI.Context;

public partial class AppDbContext : DbContext
{
    public DbSet<TypesenseKey> TypesenseKeys => Set<TypesenseKey>();
    
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
}