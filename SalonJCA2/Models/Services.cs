using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalonJCA2.Models
{
    public class Services
    {
        [Key]
        public int id { get; set; }
        public string Name { get; set; }
        public int Productid { get; set; }
        public int Typeid { get; set; }
        public int Price { get; set; }
        public string path { get; set; }
        [NotMapped]
        public string TypeName { get; set; }

    }
}
