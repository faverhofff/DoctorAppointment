using System.ComponentModel.DataAnnotations;

namespace DoctorAppointmentApi.Requests
{
    public class CreateDoctorRequest
    {
        [Required]
        public string Name { get; set; }
    }
}
