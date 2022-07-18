
using Mango.Services.OrderAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.OrderAPI.DbContexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> option) : base(option)
        {

        }
        public DbSet<OrderHeader>  orderHeaders { get; set; }
        public DbSet<OrderDetails> orderDetails  { get; set; }
        //public DbSet<CartDetails> CartDetails { get; set; }
    }
}
