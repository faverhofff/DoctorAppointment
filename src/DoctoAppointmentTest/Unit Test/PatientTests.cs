using DoctorAppointmentApi.Exceptions;
using DoctorAppointmentApi.Services;
using DoctorAppointmentTest.Helper;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DoctorAppointmentTest
{

    public class PatientTests : DatabaseFixture
    {
        private readonly IDoctorServices _doctorServices;
        private readonly IPatientServices _patientServices;
        private readonly DatabaseFixture fixture;

        public PatientTests()
        {
            _doctorServices = new DoctorServices(_mongoDbContext);
            _patientServices = new PatientServices(_mongoDbContext);
        }

        /// <summary>
        /// 1) Creating patient and check that was created if has Id value
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task create_patient_test()
        {
            /// (1)
            var randomName = Guid.NewGuid().ToString();
            var patient = await _patientServices.CreatePatient(randomName).ConfigureAwait(false);

            Assert.NotNull(patient.Id);
            Assert.Equal(randomName, patient.Name);
        }

        /// <summary>
        /// 1) Create 2 doctors, 2 patients and 3 appointments
        /// 2) Check, patient 1 has 1 appointment, patient 2 has 2 appointments
        /// 3) If doctor id not found, outut is empty array
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task get_appointments_by_patient_test()
        {
            /// (1)
            var doctor01 = await _doctorServices.CreateDoctor("DOCTOR-2").ConfigureAwait(false);
            var doctor02 = await _doctorServices.CreateDoctor("DOCTOR-3").ConfigureAwait(false);
            var patient01 = await _patientServices.CreatePatient("PATIENT-2").ConfigureAwait(false);
            var patient02 = await _patientServices.CreatePatient("PATIENT-3").ConfigureAwait(false);

            await _doctorServices.CreateAppointment(doctor01.Id, patient01.Id, DateTime.Now).ConfigureAwait(false);
            await _doctorServices.CreateAppointment(doctor01.Id, patient02.Id, DateTime.Now).ConfigureAwait(false);
            await _doctorServices.CreateAppointment(doctor02.Id, patient02.Id, DateTime.Now).ConfigureAwait(false);

            /// (2)
            var patient01Appointments = _patientServices.GetAppointments(patient01.Id).ToList();
            var patient02Appointments = _patientServices.GetAppointments(patient02.Id).ToList();
            Assert.Equal(1, patient01Appointments.Count);
            Assert.Equal(patient01.Id, patient01Appointments.First().PatientId);
            Assert.Equal(2, patient02Appointments.Count);
            Assert.Equal(patient02.Id, patient02Appointments.First().PatientId);

            /// (3)
            var patient03Appointments = _patientServices.GetAppointments("63e7dba8e7535469a771779a").ToList();
            Assert.Equal(0, patient03Appointments.Count);
        }

        /// <summary>
        /// 1) If appointment do not exists, lauch exception
        /// 2) Creating new doctor, patient and appointment for them
        /// 3) If apointment exists, cancel it.
        /// 4) Check for that doctor if the already deleted appointment do not exists
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task cancel_appointment_all_exceptions_test()
        {
            /// (1)
            Assert.ThrowsAsync<AppointmentNotFoundException>(async () => await _patientServices.CancelAppointment("63e7dba8e7535469a771779a").ConfigureAwait(false));

            /// (2)
            var randomName = Guid.NewGuid().ToString();
            var doctor = await _doctorServices.CreateDoctor("DOCTOR-2").ConfigureAwait(false);
            var patient = await _patientServices.CreatePatient(randomName).ConfigureAwait(false);
            var appointment = await _doctorServices.CreateAppointment(doctor.Id, patient.Id, DateTime.Now).ConfigureAwait(false);
            Assert.NotNull(appointment.Id);
            var lastId = new string(appointment.Id);

            /// (3)
            await _patientServices.CancelAppointment(appointment.Id).ConfigureAwait(false);

            /// (4)
            var appointments = _doctorServices.GetAppointments(doctor.Id);
            foreach (var _appointment in appointments)
                Assert.NotEqual(_appointment.Id, lastId);

            Assert.ThrowsAsync<AppointmentNotFoundException>(async () => await _patientServices.CancelAppointment(lastId).ConfigureAwait(false));
        }

        
    }

}
