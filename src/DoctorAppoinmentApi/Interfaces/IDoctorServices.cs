using DoctorAppointmentApi.Exceptions;
using DoctorAppointmentDataLayer.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DoctorAppointmentApi.Services
{
    public interface IDoctorServices
    {
        
        /// <summary>
        /// Retrieve appointments from doctorId
        /// </summary>
        /// <param name="patientId"></param>
        /// <returns>IEnumerable<Appointment></returns>
        public IEnumerable<Appointment> GetAppointments(string doctorId);

        /// <summary>
        /// Get if doctor is available in certain date
        /// </summary>
        /// <param name="doctorId"></param>
        /// <param name="date"></param>
        /// <returns>bool</returns>
        public bool IsAvailableFor(string doctorId, DateTime date) ;

        /// <summary>
        /// Handle change appointment date
        /// </summary>
        /// <param name="appointmentId"></param>
        /// <param name="newDate"></param>
        /// <returns>Task<Appointment></returns>
        /// <exception cref="AppointmentNotFoundException"></exception>
        /// <exception cref="DoctorNotAvailableException"></exception>
        public Task<Appointment> ChangeDateForAppointment(string appointmentId, DateTime newDate);

        /// <summary>
        /// Create new appointment
        /// </summary>
        /// <param name="doctorId"></param>
        /// <param name="patientId"></param>
        /// <param name="date"></param>
        /// <returns>Task<Appointment></returns>
        /// <exception cref="DoctorNotAvailableException"></exception>
        public Task<Appointment> CreateAppointment(string doctorId, string patientId, DateTime date);

        /// <summary>
        /// Create new Doctor
        /// </summary>
        /// <param name="doctorName"></param>
        /// <returns>Task<Doctor></returns>
        public Task<Doctor> CreateDoctor(string doctorName);
    }

    
}
