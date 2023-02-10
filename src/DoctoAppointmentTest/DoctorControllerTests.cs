using DoctorAppointment;
using DoctorAppointmentApi.Requests;
using DoctorAppointmentApi.Services;
using DoctorAppointmentDataLayer.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DoctoAppointmentTest
{
    public class DoctorControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public DoctorControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task Create_Doctor_Endpoint_Test()
        {
            var content = BuildBody(new CreateDoctorRequest() { Name = "Doctor_1" });
            var response = await _client.PostAsync("/api/doctor", content).ConfigureAwait(false);
            AssertResponse(response);
        }

        [Fact]
        public async Task Get_List_Appointments_Doctor_Endpoint_Test()
        {
            var response = await _client.GetAsync("/api/doctor/appointments/63e5ec2aec198c6fce4be8c4").ConfigureAwait(false);
            AssertResponse(response);
        }

        private void AssertResponse(HttpResponseMessage response)
        {
            response.EnsureSuccessStatusCode();
            Assert.Equal("application/json; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
        }

        private StringContent BuildBody(object obj)
        {
            var stringPayload = JsonConvert.SerializeObject(obj);
            return new StringContent(stringPayload, Encoding.UTF8, "application/json");
        }
    }
}