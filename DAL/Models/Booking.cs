using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace DAL.Models
{
    [Table("Booking")]
    public class Booking
    {
        public int id { get; set; }
        public int userId { get; set; }
        public int clientId { get; set; }
        public int hallId { get; set; }
        public int totalPrice { get; set; }
        public int durationHours { get; set; }
        public DateTime dateTime { get; set; }
        public List<Service> services { get; set; }

        public Booking()
        {
            services = new List<Service>();
        }

        protected void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Booking>().HasOptional(b => b.services);
            OnModelCreating(modelBuilder);
        }
    }
}
