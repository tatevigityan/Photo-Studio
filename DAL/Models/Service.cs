using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace DAL.Models
{
    [Table("Service")]
    public class Service
    {
        public int id { get; set; }
        public string name { get; set; }
        public int hourlyPrice { get; set; }
        public List<Booking> bookings { get; set; }

        protected void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Service>().HasOptional(s => s.bookings);
            OnModelCreating(modelBuilder);
        }
    }
}
