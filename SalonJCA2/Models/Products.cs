using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalonJCA2.Models
{
    public class Products
    {
        [Key]
        public int id { get; set; }
        public string Name { get; set; }

        //public ICollection<Types> types { get; set; }

  

    }
}
