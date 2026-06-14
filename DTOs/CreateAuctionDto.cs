namespace PokeBid.API.DTOs
{
    public class CreateAuctionDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty; 
        public string Condition { get; set; } = string.Empty;
        
        public decimal StartingPrice { get; set; }
        public DateTime EndDate { get; set; }
        public int OwnerId { get; set; }
    }
}