using System;

namespace DoctorAppointmentApi.Exceptions
{
    [Serializable]
    public class PatientNotFoundException : Exception
    {
        public PatientNotFoundException() : base("Patient not found.")
        {
        }
    }
}
