using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class ownerLogin
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
