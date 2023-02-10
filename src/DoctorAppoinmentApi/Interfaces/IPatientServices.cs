using DoctorAppointmentApi.Exceptions;
using DoctorAppointmentDataLayer.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DoctorAppointmentApi.Services
{
    public interface IPatientServices
    {
        /// <summary>
        /// Retrieve appointments from patiendId
        /// </summary>
        /// <param name="patientId"></param>
        /// <returns>IEnumerable<Appointment></returns>
        public IEnumerable<Appointment> GetAppointments(string patientId);

        /// <summary>
        /// Cancel/Remove appointment
        /// </summary>
        /// <param name="appointmentId"></param>
        /// <returns>Task<bool></returns>
        /// <exception cref="AppointmentNotFoundException"></exception>
        public Task<bool> CancelAppointment(string appointmentId);

        /// <summary>
        /// Create new Patient
        /// </summary>
        /// <param name="patientName"></param>
        /// <returns>Task<Patient></returns>
        public Task<Patient> CreatePatient(string patientName);
    }
}
