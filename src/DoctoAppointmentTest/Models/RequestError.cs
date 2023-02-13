using System.Collections.Generic;

namespace DoctorAppointmentTest.Models
{
    public class RequestError
    {
        public string type { get; set; }
        public string title { get; set; }
        public string status { get; set; }
        public string traceId { get; set; }
        public IDictionary<string, IList<string>> errors { get; set; }
    }
}
