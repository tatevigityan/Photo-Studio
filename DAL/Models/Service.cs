using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    [Table("Service")]
    public class Service
    {
        public int id { get; set; }
        public string name { get; set; }
        public string imageTitle { get; set; }
        public int hourlyPrice { get; set; }
    }
}
