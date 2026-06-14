using PokeBid.API.DTOs;
using PokeBid.API.Repositories;

namespace PokeBid.API.Services
{
    public class AuctionService : IAuctionService
    {
        private readonly IAuctionRepository _repository;

        public AuctionService(IAuctionRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<AuctionDto>> GetAllAuctionsAsync(string? category)
        {
            var auctions = await _repository.GetAllAsync(category);

            return auctions.Select(a => new AuctionDto
            {
                Id = a.Id,
                Title = a.Title,
                Description = a.Description,
                Category = a.Category,
                Condition = a.Condition,
                StartingPrice = a.StartingPrice,
                CurrentHighestBid = a.CurrentHighestBid,
                StartDate = a.StartDate,
                EndDate = a.EndDate,
                OwnerId = a.OwnerId
            });
        }

        public async Task<AuctionDto> CreateAuctionAsync(CreateAuctionDto createDto)
        {
            var auction = new Models.Auction
            {
                Title = createDto.Title,
                Description = createDto.Description,
                Category = createDto.Category,
                Condition = createDto.Condition,
                StartingPrice = createDto.StartingPrice,
                StartDate = DateTime.UtcNow,
                EndDate = createDto.EndDate,
                OwnerId = createDto.OwnerId
            };

            await _repository.AddAsync(auction);
            await _repository.SaveChangesAsync();

            return new AuctionDto
            {
                Id = auction.Id,
                Title = auction.Title,
                Description = auction.Description,
                Category = auction.Category,
                Condition = auction.Condition,
                StartingPrice = auction.StartingPrice,
                CurrentHighestBid = auction.CurrentHighestBid,
                StartDate = auction.StartDate,
                EndDate = auction.EndDate,
                OwnerId = auction.OwnerId
            };
        }

        // walidacja i przetwarzanie nowej oferty
        public async Task<bool> PlaceBidAsync(int auctionId, CreateBidDto dto)
        {
            var auction = await _repository.GetByIdAsync(auctionId);
            if (auction == null) return false;

            if (DateTime.UtcNow > auction.EndDate)
            {
                throw new InvalidOperationException("Aukcja już się zakończyła.");
            }

            var currentPrice = auction.CurrentHighestBid ?? auction.StartingPrice;
            if (dto.Amount <= currentPrice)
            {
                throw new InvalidOperationException("Nowa oferta musi być wyższa niż obecna cena.");
            }

            auction.CurrentHighestBid = dto.Amount;
            await _repository.UpdateAsync(auction);

            var bid = new Models.Bid
            {
                Amount = dto.Amount,
                CreatedAt = DateTime.UtcNow,
                AuctionId = auctionId,
                UserId = dto.UserId
            };
            
            await _repository.AddBidAsync(bid);
            await _repository.SaveChangesAsync();
            
            return true;
        }

        public async Task<AuctionDto?> GetAuctionByIdAsync(int id)
        {
            var a = await _repository.GetByIdAsync(id);
            if (a == null) return null;

            return new AuctionDto { Id = a.Id, Title = a.Title, Description = a.Description, Category = a.Category, Condition = a.Condition, StartingPrice = a.StartingPrice, CurrentHighestBid = a.CurrentHighestBid, StartDate = a.StartDate, EndDate = a.EndDate, OwnerId = a.OwnerId };
        }

        public async Task<bool> UpdateAuctionAsync(int id, CreateAuctionDto updateDto)
        {
            var auction = await _repository.GetByIdAsync(id);
            if (auction == null) return false;

            auction.Title = updateDto.Title;
            auction.Description = updateDto.Description;
            auction.Category = updateDto.Category;
            auction.Condition = updateDto.Condition;
            
            await _repository.UpdateAsync(auction);
            await _repository.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteAuctionAsync(int id)
        {
            var auction = await _repository.GetByIdAsync(id);
            if (auction == null) return false;

            await _repository.DeleteAsync(auction);
            await _repository.SaveChangesAsync();
            return true;
        }
    }
}