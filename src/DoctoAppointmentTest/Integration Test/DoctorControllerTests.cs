using DoctorAppointment;
using DoctorAppointmentApi.Requests;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace DoctorAppointmentTest
{
    public class DoctorControllerTests : Helpers, IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public DoctorControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task create_doctor_test()
        {
            var content = BuildBody(new CreateDoctorRequest() { Name = "Doctor_1" });
            var response = await _client.PostAsync("/api/doctor", content).ConfigureAwait(false);
            AssertResponse(response);
        }

        [Fact]
        public async Task create_doctor_without_name_test()
        {
            var content = BuildBody(new CreateDoctorRequest());
            var response = await _client.PostAsync("/api/doctor", content).ConfigureAwait(false);
            AssertRequestValidationResponse(response, "Name");
        }

        [Fact]
        public async Task get_list_appointments_doctor_test()
        {
            var response = await _client.GetAsync("/api/doctor/appointments/63e5ec2aec198c6fce4be8c4").ConfigureAwait(false);
            AssertResponse(response);
        }


    }
}