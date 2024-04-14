using System.ComponentModel.DataAnnotations;

namespace SalonJCA2.Models
{
    public class Times
    {
        [Key]
        public int Id { get; set; } 
        public string timeRang{ get; set; }
    }
}
