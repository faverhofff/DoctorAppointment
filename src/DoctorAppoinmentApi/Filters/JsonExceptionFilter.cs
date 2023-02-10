using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using System.Net.Http;

namespace DoctorAppointmentApi.Filters
{
    public class JsonExceptionFilter : ExceptionFilterAttribute
    {
        //public override void OnException(HttpActionExecutedContext context)
        //{
        //    context.Response = new HttpResponseMessage(HttpStatusCode.BadRequest);
        //}
    }
}
