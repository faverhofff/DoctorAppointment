using System;

namespace DoctorAppointmentApi.Exceptions
{
    [Serializable]
    public class DoctorNotAvailableException : Exception
    {
        public DoctorNotAvailableException() : base("Doctor not available for date specified.")
        {
        }

        //public override void OnException(ExceptionContext context)
        //{
        //    var exception = context.Exception;
        //    context.Result = new JsonResult(exception.Message);
        //}
    }
}
