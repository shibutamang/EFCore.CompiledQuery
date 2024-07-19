using Microsoft.EntityFrameworkCore;
using PreCompiledQuery.Entities;

namespace PreCompiledQuery.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        public virtual DbSet<Order> Orders { get; set; }
    }
}
