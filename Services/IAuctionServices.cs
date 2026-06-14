using PokeBid.API.DTOs;

namespace PokeBid.API.Services
{
    public interface IAuctionService
    {
        Task<IEnumerable<AuctionDto>> GetAllAuctionsAsync(string? category);
        Task<AuctionDto?> GetAuctionByIdAsync(int id);
        Task<AuctionDto> CreateAuctionAsync(CreateAuctionDto createDto);
        Task<bool> UpdateAuctionAsync(int id, CreateAuctionDto updateDto);
        Task<bool> DeleteAuctionAsync(int id);
        Task<bool> PlaceBidAsync(int auctionId, CreateBidDto dto);
    }
}