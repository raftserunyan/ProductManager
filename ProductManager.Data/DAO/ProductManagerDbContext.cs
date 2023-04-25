using Microsoft.EntityFrameworkCore;
using ProductManager.Data.Entities;

namespace ProductManager.Data.DAO
{
    public class ProductManagerDbContext : DbContext
    {
        public ProductManagerDbContext(DbContextOptions<ProductManagerDbContext> options) : base(options)
        {
        }

        public DbSet<ProductEntity> Products { get; set; }
    }
}
