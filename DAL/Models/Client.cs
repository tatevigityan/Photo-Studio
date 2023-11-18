using System.Data.Entity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    [Table("Client")]
    public class Client
    {
        public int id { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string phone { get; set; }
        public List<Booking> reservations { get; set; }
        public List<StudioServiceMembership> services { get; set; }

        public Client()
        {
            services = new List<StudioServiceMembership>();
        }

        protected void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>().HasOptional(client => client.services);
            OnModelCreating(modelBuilder);
        }
    }
}