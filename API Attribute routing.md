1. When you create an API in .NET we  will have a WebAPiConfig file that will have MapHttpRoutes defined so that our UIRs can be configured right out of ther box. We also have atrribute rrouting included and this is what we will use below. The initial map is nice to use if you are going to have the 4 four basic crud operations, the issue will begin to appear when you have more that one get request invovling a query param.
```cs
  public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
```
2. Notice how the attribute routes differ, sometimes it is enough to specify the data type,  as we did below, but this still resolved in an error, I finally had to add an additional attrobute "rate" to the route in order to resolve the endpoints with no error.
```cs

        [HttpGet]
        [Route("api/Contractor/{id:int}")]
        public async Task<IHttpActionResult> GetContractor([FromUri]int id)
        {
            var contractor = await _db.Contractors.FindAsync(id);
            return Ok(contractor);
        }

        [HttpGet]
        [Route("api/Contractor/rate/{rate:double}")]
        public async Task<IHttpActionResult> GetCheapContractors([FromUri]double rate)
        {
            var cheap = await _db.Contractors.Where(c => c.Rate < rate).ToListAsync();
            return Ok(cheap);
        }
```
