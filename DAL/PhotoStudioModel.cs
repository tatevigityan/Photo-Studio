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

        public virtual DbSet<StudioHall> studioHalls { get; set; }
        public virtual DbSet<Client> clients { get; set; }
        public virtual DbSet<User> users { get; set; }
        public virtual DbSet<Booking> bookings { get; set; }
        public virtual DbSet<StudioService> studioServices { get; set; }
        public virtual DbSet<StudioServiceMembership> studioServiceMemberships { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}