1. below is the model for Countries. The Authors prop isd used to establish a many to many realtionship and will be returned each time we quesry for a country. to eliminate that prop on return, we will create a Dto.
```cs
namespace APILegacy.Models
{
    public class Country
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public virtual ICollection<Author> Authors { get; set; }
    }
}
```
2. In the main directory create a new directory titled Dtos. Inside of Dtos, create a new class titled CountryDto
```cs
namespace APILegacy.Dtos
{
    public class CountryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
```
3. Now in our controller we will query for all countires in the db and return them. for each country returned, we will add an elemnt to a list of CountryDto and return that list imstead of the data queired fro originaly. Essentialy the Dto data will be returned instead of the actual country model data.
```cs

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
```
4. Below is how you implement a Dto that does not use a loop
```cs
    [HttpGet, Route("{bookId:int}")]
        public async Task<IHttpActionResult> GetCategory(int categoryId)
        {
            if (Equals(!ModelState.IsValid))
            {
                return BadRequest();
            }

            if (!_repository.CategoryExists(categoryId))
            {
                return NotFound();
            }

            var category = await _repository.GetCategory(categoryId);

            var categoryDto = new CategoryDto()
            {
                Id = category.Id,
                Name = category.Name
            };

            return Ok(categoryDto);
        }
```
