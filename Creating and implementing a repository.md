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
        bool CountryExists(int countryId);
        bool IsDuplicateCountry(string countryName);
        void CreateCountry(Country country);
        void UpdateCountry(Country country, int countryId);
        void DeleteCountry(int countryId);
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

        public bool CountryExists(int countryId)
        {
            return _db.Countries.Any(c => c.Id == countryId);
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
        public bool IsDuplicateCountry(string countryName)
        {
            var country = _db.Countries.Any(c => c.Name == countryName);

            if (country)
            {
                return true;
            }

            return false;
        }

        public void CreateCountry(Country country)
        {
            _db.Countries.Add(country);
            _db.SaveChanges();
        }

        public void UpdateCountry(Country country, int countryId)
        {
            var countryToUpdate = _db.Countries.Find(countryId);

            if (countryToUpdate != null)
            {
                countryToUpdate.Name = country.Name;
                _db.SaveChanges();
            }
        }

        public void DeleteCountry(int countryId)
        {
            _db.Countries.Remove(_db.Countries.Find(countryId));
            _db.SaveChanges();
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
        public async Task<IHttpActionResult> GetCountries()
        {
            var countries = await _repository.GetCountries();

            if (Equals(!ModelState.IsValid))
            {
                return BadRequest();
            }

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

        [HttpGet, Route("{countryId:int}")]
        public async Task<IHttpActionResult> GetCountry(int countryId)
        {
            if (Equals(!ModelState.IsValid))
            {
                return BadRequest();
            }

            if (! _repository.CountryExists(countryId))
            {
                return NotFound();
            }

            var country = await _repository.GetCountry(countryId);

            var countryDto = new CountryDto()
            {
                Id = country.Id,
                Name = country.Name
            };

            return Ok(countryDto);
        }

        [HttpGet, Route("authors/{authorId:int}")]
        public async Task<IHttpActionResult> GetAuthorCountry(int authorId)
        {
            if (Equals(!ModelState.IsValid))
            {
                return BadRequest();
            }

            var country = await _repository.GetAuthorCountry(authorId);

            var countryDto = new CountryDto()
            {
                Id = country.Id,
                Name = country.Name
            };

            return Ok(countryDto);
        }

        [HttpGet, Route("{countryId:int}/authors")]
        public async Task<IHttpActionResult> GetAuthorsFromCountry(int countryId)
        {
            if (Equals(!ModelState.IsValid))
            {
                return BadRequest();
            }

            if (_repository.CountryExists(countryId))
            {
                var authors = await _repository.GetAuthorsFromCountry(countryId);

                var authorsDto = new List<AuthorDto>();

                foreach (var author in authors)
                {
                    authorsDto.Add(new AuthorDto { 

                        Id = author.Id,
                        FirstName = author.FirstName,
                        LastName = author.LastName
                    });
                }

                return Ok(authorsDto);
            }

            return NotFound();
        }

        [HttpPost, Route("")]
        public IHttpActionResult CreateCountry([FromBody]Country country)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            _repository.CreateCountry(country);

            return Ok();
        }

        [HttpDelete, Route("{countryId:int}")]
        public IHttpActionResult DeleteCountry(int countryId)
        {
            if (!_repository.CountryExists(countryId))
            {
                return NotFound();
            }

            _repository.DeleteCountry(countryId);

            return Ok();
        }

        [HttpPut, Route("{countryId:int}")]
        public IHttpActionResult UpdateCountry([FromBody]Country country, int countryId)
        {

            _repository.UpdateCountry(country, countryId);

            return Ok();
        }
    }
}
```
