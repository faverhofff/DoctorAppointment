using System;
using System.ComponentModel.DataAnnotations;

namespace DoctorAppointmentApi.Requests
{
    public class UpdateAppointmentRequest
    {
        [Required]
        public string? AppointmentId { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Schedule { get; set; }
    }
}
