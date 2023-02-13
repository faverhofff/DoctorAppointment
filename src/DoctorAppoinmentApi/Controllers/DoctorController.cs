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

        /// <summary>
        /// Get appointments from doctor endpoint
        /// </summary>
        /// <param name="doctorId"></param>
        /// <returns></returns>
        [HttpGet("appointments/{doctorId}")]
        public async Task<ActionResult<IEnumerable<Appointment>>> ListAppointments(string doctorId)
        {
            var appointments = _doctorService.GetAppointments(doctorId);
            return Ok(appointments);
        }

        /// <summary>
        /// Create doctor endpoint
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Doctor>> CreateDoctor([FromBody] CreateDoctorRequest request)
        {
            var doctor = await _doctorService.CreateDoctor(request.Name).ConfigureAwait(false);
            return Ok(doctor);
        }
    }
}
