using System.ComponentModel.DataAnnotations;

namespace SalonJCA2.Models
{
    public class Bookings
    {
        [Key]
        public int id { get; set; }
        public int customerid { get; set; }
        public DateTime Date { get; set; }
        public int serviceid { get; set; }
     
        public string Time { get; set; }
    }
}
