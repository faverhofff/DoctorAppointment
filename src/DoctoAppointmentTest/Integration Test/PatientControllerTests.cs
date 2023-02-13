using DoctorAppointment;
using DoctorAppointmentApi.Exceptions;
using DoctorAppointmentApi.Requests;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace DoctorAppointmentTest
{
    public class PatientControllerTests : Helpers, IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public PatientControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task create_patient_without_name_test()
        {
            var content = BuildBody(new CreatePatientRequest());
            var response = await _client.PostAsync("/api/patient", content).ConfigureAwait(false);
            AssertRequestValidationResponse(response, "Name");
        }

        [Fact]
        public async Task get_List_appointments_patient_test()
        {
            var response = await _client.GetAsync("/api/patient/appointments/63e80db35938042eaa377253").ConfigureAwait(false);
            AssertExceptionResponse(response, new PatientNotFoundException());
        }

        [Fact]
        public async Task create_appointment_without_doctor_paremeter_test()
        {
            var body = BuildBody(new CreateAppointmentRequest()
            {
                PatientId = "63e5ec3eec198c6fce4be8c5",
                Schedule = DateTime.Now
            });

            var response = await _client.PostAsync("/api/patient/appointments", body).ConfigureAwait(false);
            AssertRequestValidationResponse(response, "DoctorId");
        }

        [Fact]
        public async Task create_appointment_without_patient_parameter_test()
        {
            var body = BuildBody(new CreateAppointmentRequest()
            {
                DoctorId = "63e5ec3eec198c6fce4be8c5",
                Schedule = DateTime.Now
            });

            var response = await _client.PostAsync("/api/patient/appointments", body).ConfigureAwait(false);
            AssertRequestValidationResponse(response, "PatientId");
        }

        [Fact]
        public async Task create_appointment_without_date_parameter_test()
        {
            var body = BuildBody(new CreateAppointmentRequest()
            {
                DoctorId = "63e5ec3eec198c6fce4be8c5",
                PatientId = "63e5ec3eec198c6fce4be8c5",
            });

            var response = await _client.PostAsync("/api/patient/appointments", body).ConfigureAwait(false);
            AssertRequestValidationResponse(response, "Schedule");
        }

        [Fact]
        public async Task update_appointment_without_appointmentid_test()
        {
            var content = BuildBody(new UpdateAppointmentRequest()
            {
                Schedule = DateTime.Now
            });

            var response = await _client.PutAsync("/api/patient/appointments", content).ConfigureAwait(false);
            AssertRequestValidationResponse(response, "AppointmentId"); 
        }

        [Fact]
        public async Task update_appointment_without_date_test()
        {
            var content = BuildBody(new UpdateAppointmentRequest()
            {
                AppointmentId = "63e5ec2aec198c6fce4be8c4",
            });

            var response = await _client.PutAsync("/api/patient/appointments", content).ConfigureAwait(false);
            AssertRequestValidationResponse(response, "Schedule");
        }

        [Fact]
        public async Task cancel_appointment_test()
        {
            var response = await _client.DeleteAsync("/api/patient/appointments/63e5ec2aec198c6fce4be8c4").ConfigureAwait(false);
            AssertExceptionResponse(response, new AppointmentNotFoundException());
        }

    }
}