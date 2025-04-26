using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SportsPro.Models.DataLayer
{
    public class SportsProContext : IdentityDbContext<User>
    {
        public SportsProContext(DbContextOptions<SportsProContext> options)
            : base(options)
        { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Technician> Technicians { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Incident> Incidents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new ConfigureProducts());
            modelBuilder.ApplyConfiguration(new ConfigureTechnicians());
            modelBuilder.ApplyConfiguration(new ConfigureCountries());
            modelBuilder.ApplyConfiguration(new ConfigureCustomers());
            modelBuilder.ApplyConfiguration(new ConfigureIncidents());
        }
    }
}