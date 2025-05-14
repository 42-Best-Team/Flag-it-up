using Microsoft.EntityFrameworkCore;
using FlagItUpApp.Models;

namespace FlagItUpApp.Data
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

    }
}
