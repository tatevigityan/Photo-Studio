using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    [Table("Booking")]
    public class Booking
    {
        public int id { get; set; }
        public int studioHallId { get; set; }
        public int userId { get; set; }
        public int totalPrice { get; set; }
        public int durationHours { get; set; }
        public DateTime date { get; set; }
        public DateTime bookingDate { get; set; }
        public Client client { get; set; }

        [NotMapped]
        public string manager { get; set; }
        [NotMapped]
        public string studioHall { get; set; }
    }
}
