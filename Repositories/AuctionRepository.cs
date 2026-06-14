using Microsoft.EntityFrameworkCore;
using PokeBid.API.Data;
using PokeBid.API.Models;

namespace PokeBid.API.Repositories
{
    public class AuctionRepository : IAuctionRepository
    {
        private readonly PokeBidDbContext _context;

        public AuctionRepository(PokeBidDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Auction>> GetAllAsync(string? category)
        {
            var query = _context.Auctions.AsQueryable();

            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(a => a.Category.ToLower() == category.ToLower());
            }

            return await query.ToListAsync();
        }

        public async Task<Auction?> GetByIdAsync(int id)
        {
            return await _context.Auctions.FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task AddAsync(Auction auction)
        {
            await _context.Auctions.AddAsync(auction);
        }

        public async Task UpdateAsync(Auction auction)
        {
            _context.Auctions.Update(auction);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(Auction auction)
        {
            _context.Auctions.Remove(auction);
            await Task.CompletedTask;
        }

        public async Task AddBidAsync(Bid bid)
        {
            await _context.Bids.AddAsync(bid);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}