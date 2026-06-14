using Microsoft.AspNetCore.Mvc;
using PokeBid.API.DTOs;
using PokeBid.API.Services;

namespace PokeBid.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuctionsController : ControllerBase
    {
        private readonly IAuctionService _auctionService;

        public AuctionsController(IAuctionService auctionService)
        {
            _auctionService = auctionService;
        }

        // GET: /api/auctions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuctionDto>>> GetAuctions([FromQuery] string? category)
        {
            var auctions = await _auctionService.GetAllAuctionsAsync(category);
            return Ok(auctions);
        }

        // POST: /api/auctions
        [HttpPost]
        public async Task<ActionResult<AuctionDto>> CreateAuction([FromBody] CreateAuctionDto createDto)
        {
            var createdAuction = await _auctionService.CreateAuctionAsync(createDto);
            return StatusCode(201, createdAuction);
        }

        // POST: /api/auctions/{id}/bids
        [HttpPost("{id}/bids")]
        public async Task<IActionResult> PlaceBid(int id, [FromBody] CreateBidDto dto)
        {
            try
            {
                var result = await _auctionService.PlaceBidAsync(id, dto);
                
                if (!result) 
                    return NotFound("Nie znaleziono aukcji.");

                return Ok("Oferta została przyjęta.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: /api/auctions/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<AuctionDto>> GetAuction(int id)
        {
            var auction = await _auctionService.GetAuctionByIdAsync(id);
            if (auction == null) return NotFound();
            return Ok(auction);
        }

        // PUT: /api/auctions/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAuction(int id, [FromBody] CreateAuctionDto updateDto)
        {
            var result = await _auctionService.UpdateAuctionAsync(id, updateDto);
            if (!result) return NotFound();
            return NoContent(); // 204 No Content
        }

        // DELETE: /api/auctions/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuction(int id)
        {
            var result = await _auctionService.DeleteAuctionAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}