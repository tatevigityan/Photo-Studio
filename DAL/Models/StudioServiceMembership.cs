using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    [Table("StudioServiceMembership")]
    public class StudioServiceMembership
    {
        public int id { get; set; }
        public int serviceId { get; set; }
        public int clientId { get; set; }
        public int bookingId { get; set; }
        public int durationHours { get; set; }
        public int totalPrice { get; set; }
        public DateTime dateBegin { get; set; }
    }
}
