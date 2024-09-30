using EngGenius.Domains;
using Microsoft.EntityFrameworkCore;

namespace EngGenius.Database
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> User { get; set; }
        public DbSet<Level> Level { get; set; }
        public DbSet<UserPermission> UserPermission { get; set; }
        public DbSet<UserHistory> UserHistory { get; set; }
        public DbSet<ActionType> ActionType { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }
}
