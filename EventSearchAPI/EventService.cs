using EventSearchAPI;
using MongoDB.Driver;
using Microsoft.Extensions.Logging;

namespace EventSearchAPI
{
    public class EventService
    {
        private readonly IMongoCollection<Event> _events;
        private readonly ILogger<EventService> _logger;

        public EventService(IConfiguration config, ILogger<EventService> logger)
        {
            _logger = logger;
            var mongoConnectionString = config.GetValue<string>("MongoDB:ConnectionString");
            var databaseName = config.GetValue<string>("MongoDB:Database");

            if (string.IsNullOrEmpty(mongoConnectionString))
            {
                _logger.LogError("MongoDB connection string is null or empty");
                throw new ArgumentNullException(nameof(mongoConnectionString), "MongoDB connection string cannot be null.");
            }

            var client = new MongoClient(mongoConnectionString);
            var database = client.GetDatabase(databaseName);
            _events = database.GetCollection<Event>("Events");

            _logger.LogInformation("MongoDB client initialized, connected to database: {Database}", databaseName);
        }

        public List<Event> Get()
        {
            var events = _events.Find(eventObj => true).ToList();
            _logger.LogInformation("Retrieved {Count} events from MongoDB", events.Count);
            return events;
        }

        public Event Get(string id)
        {
            var eventObj = _events.Find(eventObj => eventObj.Id == id).FirstOrDefault();
            if (eventObj == null)
            {
                _logger.LogWarning("No event found with ID: {Id}", id);
            }
            return eventObj;
        }

        public Event Create(Event newEvent)
        {
            _events.InsertOne(newEvent);
            _logger.LogInformation("Inserted new event with ID: {Id}", newEvent.Id);
            return newEvent;
        }

        public void Update(string id, Event updatedEvent)
        {
            var result = _events.ReplaceOne(eventObj => eventObj.Id == id, updatedEvent);
            _logger.LogInformation("Updated event with ID: {Id}, MatchedCount: {MatchedCount}", id, result.MatchedCount);
        }

        public void Remove(string id)
        {
            var result = _events.DeleteOne(eventObj => eventObj.Id == id);
            _logger.LogInformation("Deleted event with ID: {Id}, DeletedCount: {DeletedCount}", id, result.DeletedCount);
        }
    }
}
