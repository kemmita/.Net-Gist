1.[FromUri] is used when you pass values through the uri and not the body such as action/verb/?=12
```cs
        [HttpGet]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> GetContractor([FromUri]int id)
        {
            var contractor = await _db.Contractors.FindAsync(id);

            if (contractor == null)
            {
                return NotFound();
            }

            return Ok(contractor);
        }
```
