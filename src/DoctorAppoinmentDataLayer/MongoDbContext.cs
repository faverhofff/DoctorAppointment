using DoctorAppointmentDataLayer.Models;
using ParkBee.MongoDb;
using System.Threading.Tasks;

namespace DoctorAppointmentDataLayer
{
    public class MongoDbContext : MongoContext
    {
        private readonly IMongoContextOptionsBuilder optionsBuilder;
        public MongoDbContext(IMongoContextOptionsBuilder optionsBuilder) : base(optionsBuilder)
        {
            this.optionsBuilder = optionsBuilder;
        }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Appointment> Appointments { get; set; }

        public IMongoContextOptionsBuilder GetOptionsBuilder() => OptionsBuilder;

        protected override void OnConfiguring()
        {
            OptionsBuilder.Entity<Patient>(entity => entity.HasKey(p => p.Id));
            OptionsBuilder.Entity<Doctor>(entity => entity.HasKey(p => p.Id));
            OptionsBuilder.Entity<Appointment>(entity => entity.HasKey(p => p.Id));
        }

    }
}
