1. When we utilize attribute routing, we have been writing out the entire route, we can use the RoutePrefix and eliminate the need for this.
```cs
[RoutePrefix("api/Contractor")]
    public class ContractorController : ApiController
    {
        private readonly ApplicationDbContext _db;

        public ContractorController()
        {
            _db = new ApplicationDbContext();
        }

        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> GetContractors()
        {
            var contractors = await _db.Contractors.ToListAsync();
            return Ok(contractors);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> GetContractor([FromUri]int id)
        {
            var contractor = await _db.Contractors.FindAsync(id);
            return Ok(contractor);
        }

        [HttpGet]
        [Route("rate/{rate:double}")]
        public async Task<IHttpActionResult> GetCheapContractors([FromUri]double rate)
        {
            var cheap = await _db.Contractors.Where(c => c.Rate < rate).ToListAsync();
            return Ok(cheap);
        }

        [HttpGet]
        [Route("cheapest")]
        public async Task<IHttpActionResult> GetCheapCheapest()
        {
            var cheap = await _db.Contractors.MinAsync(c => c.Rate);
            var contracotr = _db.Contractors.Where(c => c.Rate.Equals(cheap));
            return Ok(contracotr);
        }

        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> CreateContractor([FromBody]Contractor contractor)
        {
            _db.Contractors.Add(contractor);
            await _db.SaveChangesAsync();
            return Ok(_db.Contractors.ToListAsync());
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> UpdateContractor([FromUri]int id, [FromBody]Contractor contractor)
        {
            var contractorToUpdate = _db.Contractors.Find(id);
            contractorToUpdate.Rate = contractor.Rate;
            contractorToUpdate.Name = contractor.Name;
            contractorToUpdate.Weapon = contractor.Weapon;
            await _db.SaveChangesAsync();
            return Ok(_db.Contractors.ToListAsync());
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> DeleteContractor(int id)
        {
            var contractor = _db.Contractors.Find(id);
            _db.Contractors.Remove(contractor);
            await _db.SaveChangesAsync();
            return Ok(_db.Contractors.ToListAsync());
        }

    }
```
