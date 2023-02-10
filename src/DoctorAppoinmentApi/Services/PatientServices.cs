using DoctorAppointmentApi.Exceptions;
using DoctorAppointmentDataLayer;
using DoctorAppointmentDataLayer.Models;
using MongoDB.Driver.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoctorAppointmentApi.Services
{
    public class PatientServices : IPatientServices
    {
        private readonly MongoDbContext _context;
        public PatientServices(MongoDbContext context) 
        { 
            _context = context;
        }

        /// <summary>
        /// Retrieve appointments from patiendId
        /// </summary>
        /// <param name="patientId"></param>
        /// <returns>IEnumerable<Appointment></returns>
        public IEnumerable<Appointment> GetAppointments(string patientId) 
            => _context.Appointments.Where(x => x.PatientId == patientId);

        /// <summary>
        /// Cancel/Remove appointment
        /// </summary>
        /// <param name="appointmentId"></param>
        /// <returns>bool</returns>
        /// <exception cref="AppointmentNotFoundException"></exception>
        public async Task<bool> CancelAppointment(string appointmentId)
        {
            var appointment = _context.Appointments.FirstOrDefault(x => x.Id.Equals(appointmentId));
            if (appointment == null)
                throw new AppointmentNotFoundException();

            await _context.Appointments.DeleteByKey(appointment.Id).ConfigureAwait(false);

            return await Task.FromResult(true).ConfigureAwait(false);
        }

        /// <summary>
        /// Create new Patient
        /// </summary>
        /// <param name="patientName"></param>
        /// <returns>Task<Patient></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task<Patient> Create(string patientName)
        {
            var newPatient = new Patient()
            {
                Name = patientName
            };

            await _context.Patients.InsertOneAsync(newPatient).ConfigureAwait(false);
            var patient = await _context.Patients.FirstOrDefaultAsync(x => x.Id == newPatient.Id).ConfigureAwait(false);

            return await Task.FromResult(newPatient).ConfigureAwait(false);
        }
    }
}
