using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DoctorAppointmentDataLayer.Models
{
    public class Patient
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        [BsonElement("name")]
        public string Name { get; set; }    
    }
}
