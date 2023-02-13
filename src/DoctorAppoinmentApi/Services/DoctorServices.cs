using DoctorAppointmentApi.Exceptions;
using DoctorAppointmentApi.Extensions;
using DoctorAppointmentDataLayer;
using DoctorAppointmentDataLayer.Extensions;
using DoctorAppointmentDataLayer.Models;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoctorAppointmentApi.Services
{
    public class DoctorServices : IDoctorServices
    {
        private readonly MongoDbContext _context;
        public DoctorServices(MongoDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieve appointments from doctorId
        /// </summary>
        /// <param name="patientId"></param>
        /// <returns>IEnumerable<Appointment></returns>
        public IEnumerable<Appointment> GetAppointments(string doctorId)
            => _context.Appointments.Where(x => x.DoctorId.Equals(doctorId));

        /// <summary>
        /// Get if doctor is available in certain date
        /// </summary>
        /// <param name="doctorId"></param>
        /// <param name="date"></param>
        /// <returns>bool</returns>
        public bool IsAvailableFor(string doctorId, DateTime date) 
            => _context.Appointments.Where(x => x.DoctorId.Equals(doctorId) && !x.Schedule.Equals(date)).Any();

        /// <summary>
        /// Handle change appointment date
        /// </summary>
        /// <param name="appointmentId"></param>
        /// <param name="newDate"></param>
        /// <returns>Task<Appointment></returns>
        /// <exception cref="AppointmentNotFoundException"></exception>
        /// <exception cref="DoctorNotAvailableException"></exception>
        public async Task<Appointment> ChangeDateForAppointment(string appointmentId, DateTime newDate)
        {
            var appointment = _context.Appointments.FirstOrDefault(x => x.Id.Equals(appointmentId));
            if (appointment == null)
                throw new AppointmentNotFoundException();

            var isAvailable = IsAvailableFor(appointment.DoctorId, newDate);
            if (!isAvailable)
                throw new DoctorNotAvailableException();

            var newAppointment = appointment.Clone();
            newAppointment.Schedule = newDate;

            var findByKeyDefinition = _context.GetFindByKeyFilterDefinition<Appointment>(appointmentId);
            await _context.Appointments.DeleteOneAsync(findByKeyDefinition).ConfigureAwait(false);
            await _context.Appointments.InsertOneAsync(newAppointment).ConfigureAwait(false);

            return await Task.FromResult(newAppointment).ConfigureAwait(false);
        }

        /// <summary>
        /// Create new appointment
        /// </summary>
        /// <param name="doctorId"></param>
        /// <param name="patientId"></param>
        /// <param name="date"></param>
        /// <returns>Task<Appointment></returns>
        /// <exception cref="DoctorNotAvailableException"></exception>
        public async Task<Appointment> CreateAppointment(string doctorId, string patientId, DateTime date)
        {
            var doctor = _context.Doctors.FirstOrDefault(x => x.Id.Equals(doctorId));
            if (doctor == null)
                throw new DoctorNotFoundException();

            var patient = _context.Patients.FirstOrDefault(x => x.Id.Equals(patientId));
            if (patient == null)
                throw new PatientNotFoundException();

            var isAvailable = IsAvailableFor(doctorId, date);
            if (isAvailable)
                throw new DoctorNotAvailableException();

            var newAppointment = new Appointment()
            {
                DoctorId = doctorId,
                PatientId = patientId,
                Schedule = date
            };

            await _context.Appointments.InsertOneAsync(newAppointment).ConfigureAwait(false);

            return await Task.FromResult(newAppointment).ConfigureAwait(false);
        }

        /// <summary>
        /// Create new Doctor
        /// </summary>
        /// <param name="doctorName"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<Doctor> CreateDoctor(string doctorName)
        {
            var newDoctor = new Doctor()
            {
                Name = doctorName
            };

            var options = new InsertOneOptions();
            options.BypassDocumentValidation = false;
            await _context.Doctors.InsertOneAsync(newDoctor, options).ConfigureAwait(false);

            return await Task.FromResult(newDoctor).ConfigureAwait(false);
        }
    }

    
}
