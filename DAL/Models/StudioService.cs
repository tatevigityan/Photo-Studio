using System.Data.Entity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Windows;

namespace DAL.Models
{
    [Table("StudioService")]
    public class StudioService
    {
        public int id { get; set; }
        public string name { get; set; }
        public int hourlyPrice { get; set; }
        public List<Booking> reservations { get; set; }
        [NotMapped]
        public Visibility closeVisibility { get; set; }

        protected void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudioService>().HasOptional(service => service.reservations);
            OnModelCreating(modelBuilder);
        }
    }
}
