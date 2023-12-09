using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    [Table("User")]
    public class User
    {
        public int id { get; set; }
        public string name { get; set; }
        public string surName { get; set; }
        public string login { get; set; }
        public string password { get; set; }
        public string role { get; set; }
    }

    public class UserData
    {
        public int id { get; set; }
        public string role { get; set; }
    }
}
