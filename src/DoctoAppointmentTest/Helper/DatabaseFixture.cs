using DoctorAppointmentDataLayer;
using MongoDB.Driver;
using ParkBee.MongoDb;
using System;

namespace DoctorAppointmentTest.Helper
{
    public  class DatabaseFixture : IDisposable
    {
        public MongoDbContext _mongoDbContext;

        public DatabaseFixture()
        {
            var mongoClient = new MongoClient(new MongoClientSettings
            {
                Server = new MongoServerAddress("localhost"),
                ServerSelectionTimeout = TimeSpan.FromSeconds(3)
            });


            var options = new MongoContextOptionsBuilder(mongoClient.GetDatabase("Test"));
            _mongoDbContext = new MongoDbContext(options);
        }

        public void Dispose()
        {
        }
    }
}
