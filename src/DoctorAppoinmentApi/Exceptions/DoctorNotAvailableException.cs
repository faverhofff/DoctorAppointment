using System;

namespace DoctorAppointmentApi.Exceptions
{
    [Serializable]
    public class DoctorNotAvailableException : Exception
    {
        public DoctorNotAvailableException() : base("Doctor not available for date specified.")
        {
        }
    }
}
