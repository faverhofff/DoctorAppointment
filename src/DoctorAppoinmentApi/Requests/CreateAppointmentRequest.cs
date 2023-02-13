using System;
using System.ComponentModel.DataAnnotations;

namespace DoctorAppointmentApi.Requests
{
    public class CreateAppointmentRequest
    {
        [Required]
        public string PatientId { get; set; }
        [Required]
        public string DoctorId { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Schedule { get; set; }
    }
}
