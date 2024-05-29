namespace WebApplication1.Models
{

    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int rating { get; set; }
        public int CampingSpotId { get; set; }
        public DateTime CreatedAt { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; } 
    }



}
