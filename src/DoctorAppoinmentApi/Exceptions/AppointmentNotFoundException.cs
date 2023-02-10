using System;

namespace DoctorAppointmentApi.Exceptions
{
    [Serializable]
    public class AppointmentNotFoundException : Exception
    {
        public AppointmentNotFoundException() : base("Appointment not found.")
        {
        }

    }
}
