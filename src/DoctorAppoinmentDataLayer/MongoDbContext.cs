using DoctorAppointmentDataLayer.Models;
using ParkBee.MongoDb;
using System.Threading.Tasks;

namespace DoctorAppointmentDataLayer
{
    public class MongoDbContext : MongoContext
    {
        public MongoDbContext(IMongoContextOptionsBuilder optionsBuilder) : base(optionsBuilder)
        {
        }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Appointment> Appointments { get; set; }

        protected override void OnConfiguring()
        {
            OptionsBuilder.Entity<Patient>(async entity => entity.HasKey(p => p.Id));
            OptionsBuilder.Entity<Doctor>(async entity => entity.HasKey(p => p.Id));
            OptionsBuilder.Entity<Appointment>(async entity => entity.HasKey(p => p.Id));
        }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    // To remove the requests to the Migration History table
        //    Database.SetInitializer<MongoDbContext>(null);
        //    // To remove the plural names   
        //    modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        //}
    }
}
