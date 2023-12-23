namespace DAL
{
    using DAL.Models;
    using System.Data.Entity;

    public class PhotoStudioModel : DbContext
    {
        public PhotoStudioModel()
            : base("name=PhotoStudioModel")
        {
        }

        public virtual DbSet<Role> roles { get; set; }
        public virtual DbSet<User> users { get; set; }
        public virtual DbSet<Hall> halls { get; set; }
        public virtual DbSet<Client> clients { get; set; }
        public virtual DbSet<Service> services { get; set; }
        public virtual DbSet<Booking> bookings { get; set; }
        public virtual DbSet<CategoryHall> hallCategories { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}