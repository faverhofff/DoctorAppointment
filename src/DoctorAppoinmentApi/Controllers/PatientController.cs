using DoctorAppointmentApi.Requests;
using DoctorAppointmentApi.Services;
using DoctorAppointmentDataLayer.Models;

using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DoctorAppointmentApi.Controllers
{
    [ApiController]
    [Route("api/patient")]
    public class PatientController : ControllerBase
    {
        private IPatientServices _patientService;
        private IDoctorServices _doctorService;

        public PatientController(IPatientServices patientService, IDoctorServices doctorService)
        {
            _patientService = patientService;
            _doctorService = doctorService;
        }

        /// <summary>
        /// Create patient endpoint
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Patient>> CreatePatient([FromBody] CreatePatientRequest request)
        {
            var newAppointment = await _patientService.CreatePatient(request.Name).ConfigureAwait(false);
            return Ok(newAppointment);
        }

        /// <summary>
        /// Get appointments from patient endpoint
        /// </summary>
        /// <param name="patientId"></param>
        /// <returns></returns>
        [HttpGet("appointments/{patientId}")]
        public async Task<ActionResult<IEnumerable<Appointment>>> ListAppointments(string patientId)
        {
            return Ok(_patientService.GetAppointments(patientId));
        }

        /// <summary>
        /// Create appointment endpoint
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("appointments")]
        public async Task<ActionResult<Appointment>> CreateAppointment([FromBody] CreateAppointmentRequest request)
        {
            var newAppointment = await _doctorService.CreateAppointment(request.DoctorId, request.PatientId, request.Schedule).ConfigureAwait(false);
            return Ok(newAppointment);
        }

        /// <summary>
        /// Change date or update appointment endpoint
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("appointments")]
        public async Task<ActionResult<Appointment>> UpdateAppointment([FromBody] UpdateAppointmentRequest request)
        {
            var newAppointmentData = await _doctorService.ChangeDateForAppointment(request.AppointmentId, request.Schedule).ConfigureAwait(false);
            return Ok(newAppointmentData);
        }

        /// <summary>
        /// Cancel/Delete appointment endpoint
        /// </summary>
        /// <param name="appointmendId"></param>
        /// <returns></returns>
        [HttpDelete("appointments/{appointmendId}")]
        public async Task<ActionResult<Appointment>> CancelAppointment(string appointmendId)
        {
            var newAppointmentData = _patientService.CancelAppointment(appointmendId);
            return Ok(newAppointmentData);
        }
    }
}
