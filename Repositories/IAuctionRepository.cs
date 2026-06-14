using PokeBid.API.Models;

namespace PokeBid.API.Repositories
{
    public interface IAuctionRepository
    {
        Task<IEnumerable<Auction>> GetAllAsync(string? category);
        Task<Auction?> GetByIdAsync(int id);
        Task AddAsync(Auction auction);
        Task UpdateAsync(Auction auction);
        Task DeleteAsync(Auction auction);
        
        Task AddBidAsync(Bid bid); 
        
        Task SaveChangesAsync();
    }
}