0. Event Class
```cs
        public int EventId { get; set; }
        public string EventName { get; set; }

        [ForeignKey("Venue")]
        public int VenueId { get; set; }
        public Venue Venue { get; set; }
        public ICollection<Gig> Gigs { get; set; }
```
0.1 EventDto Class
```cs
public class EventDto
    {
        public int EventId { get; set; }
        public string EventName { get; set; }
    }
```
1. We will first need to add a new Nugget Package.
```
AutoMapper
```
2. In Global.asax.cs add this code in Application_Start Method
```cs
MapperConfig.RegisterMapping();
```
3. Create a new directory titled Common and in this directory, you will want to create a new class titled "MapperConfig.cs"
```cs
    public static class MapperConfig
    {
        public static void RegisterMapping()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Event, EventDto>().ReverseMap();
            });
        }
    }
```
4. Below we our in the EventsController and will be returning a list back from the Events Service Repo.
```cs
    [HttpGet, Route("")]
        public async Task<IHttpActionResult> GetEvents()
        {
                var events = await _repository.GetEvents(true);
               
                var dataToReturn = AutoMapper.Mapper.Map(events, new List<EventDto>());

                return Ok(dataToReturn);
        }
```
