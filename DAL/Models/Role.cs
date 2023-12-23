using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    [Table("Role")]
    public class Role
    {
        public int id { get; set; }
        public string name { get; set; }
    }
}