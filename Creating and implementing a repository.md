1. Create a new directory called Services, in here we will place all repositories, interfaces for the repositoires and db context.
2. In services direcotry create an interface titled ICountryRepository
```cs
namespace APILegacy.Services
{
    public interface ICountryRepository
    {
        Task<ICollection<Country>> GetCountries();
        Task<Country> GetCountry(int countryId);
        Task<Country> GetAuthorCountry(int authorId);
        Task<ICollection<Author>> GetAuthorsFromCountry(int countryId);
        Task<bool> CountryExists(int countryId);
    }
}
```
3. Next you will create the CountryRepositroy to implement the interface. Create this in the same direcotyr.
```cs
namespace APILegacy.Services
{
    public class CountryRepository : ICountryRepository
    {
        private readonly ApplicationDbContext _db;

        public CountryRepository()
        {
            _db = new ApplicationDbContext();
            _db.Configuration.ProxyCreationEnabled = false;
        }

        public async Task<bool> CountryExists(int countryId)
        {
            return await _db.Countries.AnyAsync(c => c.Id == countryId);
        }

        public async Task<ICollection<Country>> GetCountries()
        {
            return await _db.Countries.OrderBy(c => c.Name).ToListAsync();
        }

        public async Task<Country> GetCountry(int countryId)
        {
            return await _db.Countries.FindAsync(countryId);
        }

        public async Task<Country> GetAuthorCountry(int authorId)
        {
            return await _db.Authors.Where(a => a.Id == authorId).Select(c => c.Country).FirstOrDefaultAsync();
        }

        public async Task<ICollection<Author>> GetAuthorsFromCountry(int countryId)
        {
            return await _db.Authors.Where(c => c.Country.Id == countryId).ToListAsync();
        }
    }
}
```
4. Now we will create our CountriesController that will utilize our repositry above.
```cs
namespace APILegacy.Controllers
{
    [RoutePrefix("api/Country")]
    public class CountriesController : ApiController
    {
        private readonly ICountryRepository _repository;

        public CountriesController()
        {
            _repository = new CountryRepository();
        }

        [HttpGet, Route("")]
        public async Task<IHttpActionResult> Get()
        {
            var countries = await _repository.GetCountries();
            var countriesDto = new List<CountryDto>();
            foreach (var country in countries)
            {
                countriesDto.Add(new CountryDto
                {
                    Id = country.Id,
                    Name = country.Name
                });
            }
            return Ok(countriesDto);
        }
    }
}
```
