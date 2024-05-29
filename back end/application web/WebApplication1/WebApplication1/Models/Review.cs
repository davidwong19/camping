namespace WebApplication1.Models
{
    public class Review
    {
        public int Id { get; set; }
        public int rating { get; set; } 
        public int UserId { get; set; }
        public User User { get; set; }
        public int CampingSpotId { get; set; }
    }

}
