using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthService.Models
{    
    public class User
    {
        [Key]
        [Required]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(25)]
        public string UserName { get; set; }
        
        [MinLength(10)]
        [MaxLength(18)]
        [Required]
        public string Password { get; set; }
        
        [MaxLength(25)]
        public string Name { get; set; }
        
        [MaxLength(10)]
        public string PhNumber { get; set; }
        
        [NotMapped]
        public string? token { get; set; }
    }
}
