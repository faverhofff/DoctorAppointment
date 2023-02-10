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
    public class PatientControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private Mock<IPatientServices> _mockPatientServices;
        private Mock<IDoctorServices> _mockIDoctorServices;

        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public PatientControllerTests(WebApplicationFactory<Program> factory)
        {
            _mockPatientServices = new Mock<IPatientServices>();
            _mockIDoctorServices = new Mock<IDoctorServices>();

            _mockPatientServices.Setup(x => x.Create("Patient_1")).Returns(
                Task.FromResult(new Patient() { Name = "Patient_1" })
            );

            _factory = factory;
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task Create_Patient_Endpoint_Test()
        {
            var content = BuildBody(new CreatePatientRequest() { Name = "Patient_1" });
            var response = await _client.PostAsync("/api/patient", content).ConfigureAwait(false);
            AssertResponse(response);
        }

        [Fact]
        public async Task Get_List_Appointments_Patient_Endpoint_Test()
        {
            var response = await _client.GetAsync("/api/patient/appointments/1").ConfigureAwait(false);
            AssertResponse(response);
        }

        [Fact]
        public async Task Create_Appointment_Endpoint_Test()
        {
            var content = BuildBody(new CreateAppointmentRequest()
            {
                DoctorId = "63e5ec2aec198c6fce4be8c4",
                PatientId = "63e5ec3eec198c6fce4be8c5",
                Schedule = DateTime.Now
            });

            var response = await _client.PostAsync("/api/patient/appointments", content).ConfigureAwait(false);
            AssertResponse(response);
        }

        [Fact]
        public async Task Update_Appointment_Endpoint_Test()
        {
            var content = BuildBody(new UpdateAppointmentRequest()
            {
                AppointmentId = "63e5ec2aec198c6fce4be8c4",
                Schedule = DateTime.Now
            });

            var response = await _client.PutAsync("/api/patient/appointments", content).ConfigureAwait(false);
            AssertResponse(response);
        }

        [Fact]
        public async Task Cancel_Appointment_Endpoint_Test()
        {
            var response = await _client.DeleteAsync("/api/patient/appointments/63e5ec2aec198c6fce4be8c4").ConfigureAwait(false);
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