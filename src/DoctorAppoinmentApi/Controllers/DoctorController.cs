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
    [Route("api/doctor")]
    public class DoctorController : ControllerBase
    {
        private IDoctorServices _doctorService;
        private readonly ILogger<DoctorController> _logger;

        public DoctorController(ILogger<DoctorController> logger, IDoctorServices doctorService)
        {
            _logger = logger;
            _doctorService = doctorService;
        }

        [HttpGet("appoinments/{doctorId}")]
        public async Task<ActionResult<IEnumerable<Appointment>>> ListAppointments(string doctorId)
        {
            var appointments = _doctorService.GetAppointments(doctorId);
            return Ok(appointments);
        }

        [HttpPost]
        public async Task<ActionResult<Doctor>> Create([FromBody] CreateDoctorRequest request)
        {
            var doctor = await _doctorService.CreateDoctor(request.Name).ConfigureAwait(false);
            return Ok(doctor);
        }
    }
}
