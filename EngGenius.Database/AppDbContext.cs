using EngGenius.Domains;
using Microsoft.EntityFrameworkCore;

namespace EngGenius.Database
{
    public class AppDbContext : DbContext
    {
        public DbSet<Person> Person {  get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }
}
