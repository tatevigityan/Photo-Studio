using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    [Table("StudioHall")]
    public class StudioHall
    {
        public int id { get; set; }
        public string name { get; set; }
        public string category { get; set; }
        public int hourlyPrice { get; set; }
    }
}