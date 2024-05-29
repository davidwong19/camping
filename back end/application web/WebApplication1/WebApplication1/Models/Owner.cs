namespace WebApplication1.Models
{
    public class Owner
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        // Foreign key
        public int OwnerId { get; set; }

        // Navigation property
        public List<CampingSpot> CampingSpots { get; set; }
    }

}
