using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalonJCA2.Models
{
    public class Types
    {
        [Key]
        public int id { get; set; }
        public string TypeName { get; set; }
            
       public int productid { get; set; }

        [NotMapped]
        public string Productname { get; set; }
        //public Items items { get; set; }
    }
}
