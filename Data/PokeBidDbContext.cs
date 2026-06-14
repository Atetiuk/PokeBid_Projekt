using Microsoft.EntityFrameworkCore;
using PokeBid.API.Models;

namespace PokeBid.API.Data
{
    public class PokeBidDbContext : DbContext
    {
        public PokeBidDbContext(DbContextOptions<PokeBidDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Auction> Auctions { get; set; }
        public DbSet<Bid> Bids { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<Auction>()
                .Property(a => a.StartingPrice)
                .HasColumnType("decimal(18,2)");
                
            modelBuilder.Entity<Bid>()
                .Property(b => b.Amount)
                .HasColumnType("decimal(18,2)");
        }
    }
}