using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    [Table("Booking")]
    public class Booking
    {
        public int id { get; set; }
        public int userId { get; set; }
        public int clientId { get; set; }
        public int hallId { get; set; }
        public int? serviceId { get; set; }
        public int totalPrice { get; set; }
        public int durationHours { get; set; }
        public DateTime dateTime { get; set; }
    }
}
