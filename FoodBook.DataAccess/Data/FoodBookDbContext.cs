using FoodBook.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodBook.DataAccess.Data
{
    public class FoodBookDbContext : DbContext
    {
        public FoodBookDbContext(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<User> Users { get; set; }
    }
}