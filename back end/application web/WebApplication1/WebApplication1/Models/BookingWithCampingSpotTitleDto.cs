namespace WebApplication1.Models
{
    public class BookingWithCampingSpotTitleDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public BookingStatus Status { get; set; }
        public int CampingSpotId { get; set; }
        public string CampingSpotTitle { get; set; }
    }
}
