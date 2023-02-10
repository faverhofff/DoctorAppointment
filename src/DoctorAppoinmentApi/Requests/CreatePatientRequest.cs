using System;
using System.ComponentModel.DataAnnotations;

namespace DoctorAppointmentApi.Requests
{
    public class CreatePatientRequest
    {
        [Required]
        public string Name { get; set; }
    }
}
