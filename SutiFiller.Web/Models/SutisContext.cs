using Microsoft.EntityFrameworkCore;

namespace SutiFiller.Web.Models
{
    public class SutisContext : DbContext
    {
        public SutisContext(DbContextOptions<SutisContext> options)
            : base(options)
        {
        }

        public DbSet<Suti> Sutis { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<SutiOrder> SutiOrders { get; set; }
        public DbSet<Guest> Guests { get; set; } = null!;
    }
}
