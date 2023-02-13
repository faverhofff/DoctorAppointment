using DoctorAppointmentTest.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DoctorAppointmentTest
{
    public class Helpers
    {

        public StringContent BuildBody(object obj)
        {
            var stringPayload = JsonConvert.SerializeObject(obj);
            return new StringContent(stringPayload, Encoding.UTF8, "application/json");
        }


        public async Task AssertExceptionResponse(HttpResponseMessage response, Exception expectedException)
        {
            var content = await response.Content.ReadAsStringAsync();
            var body = JsonConvert.DeserializeObject<ResponseError>(content);

            Assert.Equal(expectedException.Message, body.Error);
        }

        public async Task AssertRequestValidationResponse(HttpResponseMessage response, string keyMustExist)
        {
            var content = await response.Content.ReadAsStringAsync();
            var body = JsonConvert.DeserializeObject<RequestError>(content);

            Assert.True(body.errors.ContainsKey(keyMustExist));
        }

        public void AssertResponse(HttpResponseMessage response)
        {
            response.EnsureSuccessStatusCode();
            Assert.Equal("application/json; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
        }

    }
}
