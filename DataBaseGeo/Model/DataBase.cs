using Microsoft.EntityFrameworkCore;
using DataBaseGeo.Model;

namespace DataBaseGeo.Model
{
    internal class DataBase : DbContext
    {
        public DbSet<Customer> Customers { get; set; } = null!;
        public DbSet<Project> Projects { get; set; } = null!;
        public DbSet<Area> Areas { get; set; } = null!;
        public DbSet<AreaPoint> AreaPoints { get; set; } = null!;
        public DbSet<Operator> Operators { get; set; } = null!;
        public DbSet<Profile> Profiles { get; set; } = null!;
        public DbSet<ProfilePoint> ProfilePoints { get; set; } = null!;
        public DbSet<Picket> Pickets { get; set; } = null!;
        private static DataBase? instance;
        public static DataBase getInstance()
        {
            if (instance == null)
            {
                instance = new DataBase();
                //instance.Database.EnsureDeleted();
                var exists = instance.Database.EnsureCreated();

                instance.Customers.Load();
                instance.Projects.Load();
                instance.AreaPoints.Load();
                instance.Areas.Load();
                instance.Operators.Load();
                instance.ProfilePoints.Load();
                instance.Profiles.Load();
                instance.Pickets.Load();
                //if (exists)
                //    instance.Customers.Add(DefaultData);
                instance.SaveChanges();
            }
            return instance;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=.;Database=DataBaseGeo;Trusted_Connection=True; TrustServerCertificate=True");
        }
    }
}
    
    

