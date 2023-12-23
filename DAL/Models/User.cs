using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    [Table("User")]
    public class User
    {
        public int id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public int roleId { get; set; }
    }

    public class UserData
    {
        public string username { get; set; }
        public string role { get; set; }
    }
}
