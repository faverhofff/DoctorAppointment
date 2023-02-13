using DoctorAppointmentApi.Exceptions;
using DoctorAppointmentApi.Services;
using DoctorAppointmentTest.Helper;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DoctorAppointmentTest
{
    public class DoctorTests : DatabaseFixture
    {
        private readonly IDoctorServices _doctorServices;
        private readonly IPatientServices _patientServices;

        public DoctorTests()
        {
            _doctorServices = new DoctorServices(_mongoDbContext);
            _patientServices = new PatientServices(_mongoDbContext);
        }

        /// <summary>
        /// Creating new doctor
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task create_doctor_test()
        {
            var randomName = Guid.NewGuid().ToString();
            var doctor = await _doctorServices.CreateDoctor(randomName).ConfigureAwait(false);

            Assert.NotNull(doctor.Id);
            Assert.Equal(randomName, doctor.Name);
        }

        /// <summary>
        /// 1) Create 2 doctors, 2 patients and 3 appointments
        /// 2) Check, doctor 1 has 2 appointment, doctor 2 has 1 appointments
        /// 3) If doctor id not found, outut is empty array
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task get_appointments_by_doctor_test()
        {
            /// (1)
            var doctor01 = await _doctorServices.CreateDoctor("DOCTOR-20").ConfigureAwait(false);
            var doctor02 = await _doctorServices.CreateDoctor("DOCTOR-30").ConfigureAwait(false);
            var patient01 = await _patientServices.CreatePatient("PATIENT-20").ConfigureAwait(false);
            var patient02 = await _patientServices.CreatePatient("PATIENT-30").ConfigureAwait(false);

            await _doctorServices.CreateAppointment(doctor01.Id, patient01.Id, DateTime.Now).ConfigureAwait(false);
            await _doctorServices.CreateAppointment(doctor01.Id, patient02.Id, DateTime.Now).ConfigureAwait(false);
            await _doctorServices.CreateAppointment(doctor02.Id, patient02.Id, DateTime.Now).ConfigureAwait(false);

            /// (2)
            var doctor01Appointments = _doctorServices.GetAppointments(doctor01.Id).ToList();
            var doctor02Appointments = _doctorServices.GetAppointments(doctor02.Id).ToList();
            Assert.Equal(2, doctor01Appointments.Count);
            Assert.Equal(patient01.Id, doctor01Appointments.First().PatientId);
            Assert.Equal(doctor01.Id, doctor01Appointments[0].DoctorId);
            Assert.Equal(doctor01.Id, doctor01Appointments[1].DoctorId);
            Assert.Equal(1, doctor02Appointments.Count);
            Assert.Equal(doctor02.Id, doctor02Appointments.First().DoctorId);

            /// (3)
            var unknowDoctorAppointments = _doctorServices.GetAppointments("63e7dba8e7535469a771779a").ToList();
            Assert.Equal(0, unknowDoctorAppointments.Count);
        }

        /// <summary>
        /// 1) If doctor not exist into database, launch exception DoctorNotFoundException
        /// 2) If patient not exist into database, launch exception PatientNotFoundException
        /// 3) If doctor and patient exists then create appointment
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task create_appointment_all_exceptions_test()
        {
            /// (1)
            var date = DateTime.Now;
            Assert.ThrowsAsync<DoctorNotFoundException>(async () => await _doctorServices.CreateAppointment("63e7dba8e7535469a771779d", "63e7dba8e7535469a771779e", date).ConfigureAwait(false));

            /// (2)
            var doctor = await _doctorServices.CreateDoctor("DOCTOR-1").ConfigureAwait(false);
            Assert.ThrowsAsync<PatientNotFoundException>(async () => await _doctorServices.CreateAppointment(doctor.Id, "63e7dba8e7535469a771779h", date).ConfigureAwait(false));

            /// (3)
            var patient = await _patientServices.CreatePatient("PATIENT-1").ConfigureAwait(false);
            var appointment = await _doctorServices.CreateAppointment(doctor.Id, patient.Id, date).ConfigureAwait(false);
            Assert.NotNull(appointment);
            Assert.Equal(doctor.Id, appointment.DoctorId);
            Assert.Equal(patient.Id, appointment.PatientId);
            Assert.Equal(date, appointment.Schedule);
        }

        /// <summary>
        /// 1) If appoint not exists, launch exception AppointmentNotFoundException
        /// 2) Creaing new appointment with new doctor and new patient
        /// 3) If try it to change the date to another one where the doctor has date with another patient, launch exception DoctorNotAvailableException
        /// 4) If doctor is available, create appointment
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task update_appointment_all_exceptions_test()
        {
            /// (1)
            var date = DateTime.Now;
            Assert.ThrowsAsync<AppointmentNotFoundException>(async () => await _doctorServices.ChangeDateForAppointment("63e7dba8e7535469a771000d", date).ConfigureAwait(false));

            /// (2)
            var doctor = await _doctorServices.CreateDoctor("DOCTOR-3").ConfigureAwait(false);
            var patient = await _patientServices.CreatePatient("PATIENT-3").ConfigureAwait(false);
            var appointment = await _doctorServices.CreateAppointment(doctor.Id, patient.Id, date).ConfigureAwait(false);
            Assert.NotNull(appointment);
            Assert.Equal(doctor.Id, appointment.DoctorId);
            Assert.Equal(patient.Id, appointment.PatientId);
            Assert.Equal(date, appointment.Schedule);

            /// (3)
            Assert.ThrowsAsync<DoctorNotAvailableException>(async () => await _doctorServices.ChangeDateForAppointment(appointment.Id, date).ConfigureAwait(false));

            /// (4)
            var newDate = DateTime.Now.AddMonths(1);
            var newAppointment = await _doctorServices.ChangeDateForAppointment(appointment.Id, newDate).ConfigureAwait(false);
            Assert.NotNull(newAppointment);
            Assert.Equal(newDate, newAppointment.Schedule);
        }
    }

}
