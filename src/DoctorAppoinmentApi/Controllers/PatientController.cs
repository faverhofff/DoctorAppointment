using DoctorAppointmentApi.Filters;
using DoctorAppointmentApi.Requests;
using DoctorAppointmentApi.Services;
using DoctorAppointmentDataLayer.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DoctorAppointmentApi.Controllers
{
    [ApiController]
    [JsonExceptionFilter]
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

        [HttpPost]
        public async Task<IActionResult> CreatePatient([FromBody] CreatePatientRequest request)
        {
            var newAppointment = await _patientService.Create(request.Name).ConfigureAwait(false);
            return Ok(newAppointment);
        }

        [HttpGet("appointments/{patientId}")]
        public async Task<ActionResult<IEnumerable<Appointment>>> ListAppointments(string patientId)
        {
            return Ok(_patientService.GetAppointments(patientId));
        }

        [HttpPost("appointments")]
        public async Task<ActionResult<Appointment>> CreateAppointment([FromBody] CreateAppointmentRequest request)
        {
            var newAppointment = await _doctorService.CreateAppointment(request.DoctorId, request.PatientId, request.Schedule).ConfigureAwait(false);
            return Ok(newAppointment);
        }

        [HttpPut("appointments")]
        public async Task<ActionResult<Appointment>> UpdateAppointment([FromBody] UpdateAppointmentRequest request)
        {
            var newAppointmentData = _doctorService.ChangeDateForAppointment(request.AppointmentId, request.Schedule);
            return Ok(newAppointmentData);
        }

        [HttpDelete("appointments/{appointmendId}")]
        public async Task<ActionResult<Appointment>> CancelAppointment(string appointmendId)
        {
            var newAppointmentData = _patientService.CancelAppointment(appointmendId);
            return Ok(newAppointmentData);
        }
    }
}
