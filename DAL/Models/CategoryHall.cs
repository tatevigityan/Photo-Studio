using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    [Table("CategoryHall")]
    public class CategoryHall
    {
        public int id { get; set; }
        public string name { get; set; }
    }
}