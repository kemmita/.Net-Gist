1. We will have our routePrefix in the controller so we do not need to worry about it in the action
```cs
[RoutePrefix("api/Contractor")]
```
2. Below is a standard practice get all and get one
```cs
        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> GetContractors()
        {
            return Ok(await _db.Contractors.ToListAsync());
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> GetContractor(int id)
        {
            var contractor = await _db.Contractors.FindAsync(id);

            if (contractor == null)
            {
                return NotFound();
            }

            return Ok(contractor);
        }
```
