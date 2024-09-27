namespace EventSearchAPI
{
    public class Event
    {
        public Event() { Id = Guid.NewGuid().ToString(); }

        public string Id { get; set; }
        public string eventName { get; set; }
        public string Organisers { get; set; }
        public string Location { get; set; }
        public string Category { get; set; }
        public DateTime Date { get; set; }
        public string Details { get; set; }
    }
}

