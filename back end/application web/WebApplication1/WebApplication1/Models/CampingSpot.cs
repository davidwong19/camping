namespace WebApplication1.Models


{
    public class CampingSpot
    {
        public int Id { get; set; }
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
        public List<byte[]> Images { get; set; } = new List<byte[]>(); 

        // Foreign key
        public int OwnerId { get; set; }
        public int CampingSpotId { get; set; }

    }

}
