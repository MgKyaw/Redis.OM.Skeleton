using Microsoft.AspNetCore.Mvc;
using Redis.OM.Searching;
using Redis.OM.Skeleton.Model;

namespace Redis.OM.Skeleton.Controllers;

[ApiController]
[Route("[controller]")]
public class PeopleController : ControllerBase
{
    private readonly RedisCollection<Person> _people;
    private readonly RedisConnectionProvider _provider;

    public PeopleController(RedisConnectionProvider provider)
    {
        _provider = provider;
        _people = (RedisCollection<Person>)_provider.RedisCollection<Person>();
    }

    [HttpPost]
    public async Task<Person> AddPerson([FromBody] Person person)
    {
        await _people.InsertAsync(person);
        return person;
    }

    [HttpGet("filterAge")]
    public IList<Person> FilterByAge([FromQuery] int minAge, [FromQuery] int maxAge)
    {
        return _people.Where(x => x.Age >= minAge && x.Age <= maxAge).ToList();
    }

    [HttpGet("filterGeo")]
    public IList<Person> FilterByGeo([FromQuery] double lon, [FromQuery] double lat, [FromQuery] double radius, [FromQuery] string unit)
    {
        return _people.GeoFilter(x => x.Address!.Location, lon, lat, radius, Enum.Parse<GeoLocDistanceUnit>(unit)).ToList();
    }
}