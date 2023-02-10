using System;

namespace DoctorAppointmentApi.Exceptions
{
    public class DoctorNotFoundException : Exception
    {
        public DoctorNotFoundException() : base("Doctor not found.")
        {
        }

        //public override void OnException(ExceptionContext context)
        //{
        //    var exception = context.Exception;
        //    context.Result = new JsonResult(exception.Message);
        //}
    }
}
