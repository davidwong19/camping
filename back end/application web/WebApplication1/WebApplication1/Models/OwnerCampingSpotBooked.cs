namespace WebApplication1.Models
{
    public class OwnerCampingSpotBooked
    {
        public int Id { get; set; }
        
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public BookingStatus Status { get; set; }

        public DateTime beginDate { get; set; }
        public DateTime endDate { get; set; }
        // Foreign key
        public int CampingSpotId { get; set; }
        // Navigation properties
        public CampingSpot CampingSpot { get; set; }
    }
}
