using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using System;
using System.Globalization;

namespace DoctorAppointmentDataLayer.Extensions
{
    public class FechaTweetsSerializer : SerializerBase<DateTime>
    {

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, DateTime value)
        {
            context.Writer.WriteString(value.ToString(CultureInfo.InvariantCulture));
        }

        public override DateTime Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var fecha = context.Reader.ReadString();
            return ConvertDate(fecha);
        }

        private DateTime ConvertDate(string fechaFormatoTwitter)
        {
            var formato = "MM/dd/yyyy HH:mm:ss"; 
            var enUS = new CultureInfo("en-US");
            var fechaConvertida = DateTime.ParseExact(fechaFormatoTwitter, formato, enUS, DateTimeStyles.None);
            return fechaConvertida;
        }
    }
}
