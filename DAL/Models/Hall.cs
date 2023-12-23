using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    [Table("Hall")]
    public class Hall
    {
        public int id { get; set; }
        public string name { get; set; }
        public string imageTitle { get; set; }
        public int hourlyPrice { get; set; }
        public int categoryId { get; set; }
    }
}