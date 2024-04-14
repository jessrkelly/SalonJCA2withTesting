using System.ComponentModel.DataAnnotations;

namespace SalonJCA2.Models
{
    public class TimeMap
    {
        [Key]
        public int id { get; set; }
        public DateTime Date { get; set; }
        public string TimeSlot { get; set; }

    }
}
