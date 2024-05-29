using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class userLogin
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
