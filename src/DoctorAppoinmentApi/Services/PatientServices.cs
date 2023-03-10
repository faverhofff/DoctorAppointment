using DoctorAppointmentApi.Exceptions;
using DoctorAppointmentDataLayer;
using DoctorAppointmentDataLayer.Extensions;
using DoctorAppointmentDataLayer.Models;
using MongoDB.Driver;
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

            var findByKeyDefinition = _context.GetFindByKeyFilterDefinition<Appointment>(appointmentId);
            await _context.Appointments.DeleteOneAsync(findByKeyDefinition).ConfigureAwait(false);

            return await Task.FromResult(true).ConfigureAwait(false);
        }

        /// <summary>
        /// Create new Patient
        /// </summary>
        /// <param name="patientName"></param>
        /// <returns>Task<Patient></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task<Patient> CreatePatient(string patientName)
        {
            var newPatient = new Patient()
            {
                Id = null,
                Name = patientName
            };

            var options = new InsertOneOptions();
            options.BypassDocumentValidation = false;
            await _context.Patients.InsertOneAsync(newPatient, options).ConfigureAwait(false);

            return await Task.FromResult(newPatient).ConfigureAwait(false);
        }



    }
}
