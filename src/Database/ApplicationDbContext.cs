using Microsoft.EntityFrameworkCore;
using src.model.Entities;

namespace src.Database
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {
        }

        public ApplicationDbContext()
        {
        }

    }
}