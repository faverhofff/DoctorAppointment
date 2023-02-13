using System;

namespace DoctorAppointmentApi.Exceptions
{
    public class DoctorNotFoundException : Exception
    {
        public DoctorNotFoundException() : base("Doctor not found.")
        {
        }
    }
}
