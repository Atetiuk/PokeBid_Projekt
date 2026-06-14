namespace PokeBid.API.Models
{
    public class Auction
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    
        public string Category { get; set; } = string.Empty; 
        public string Condition { get; set; } = string.Empty;

        public decimal StartingPrice { get; set; }
        public decimal? CurrentHighestBid { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int OwnerId { get; set; }
        public User? Owner { get; set; }

        public ICollection<Bid> Bids { get; set; } = new List<Bid>();
    }
}