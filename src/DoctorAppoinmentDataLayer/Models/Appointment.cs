using DoctorAppointmentDataLayer.Extensions;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace DoctorAppointmentDataLayer.Models
{
    public class Appointment
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        [BsonElement("patient_id")]
        public string PatientId { get; set; }
        [BsonElement("doctor_id")]
        public string DoctorId { get; set; }

        [BsonElement("date_at")]
        [BsonSerializer(typeof(FechaTweetsSerializer))]
        public DateTime Schedule { get; set; }
    }
}
