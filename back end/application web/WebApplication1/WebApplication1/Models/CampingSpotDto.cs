namespace WebApplication1.Models
{
    public class CampingSpotDto
    {
      
        public string title { get; set; }
        public string country { get; set; }
        public string Location { get; set; }
        public string theme { get; set; }
        public int howmanycampers { get; set; }

        public string onsiteaccommodation { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
        public bool IsAvailable { get; set; }
        public int OwnerId { get; set; }
        public List<IFormFile> Images { get; set; } 
    }
}
