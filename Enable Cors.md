1. To enable CORS we will first need to install a Nuget package.
```
Install-Package Microsoft.AspNet.WebApi.Cors
```
2. Now go into WebApiConfig.cs and make sure your Register method appears as below!
```cs
 public static void Register(HttpConfiguration config, IWindsorContainer container)
        {
            config.EnableCors();
            MapRoutes(config);
            RegisterControllerActivator(container);
        }
```
3. Now go into the controller and add this piece of code.
```cs
namespace APILegacy.Controllers
{
    [EnableCors(origins: "http://localhost:63342", headers: "*", methods: "*")]
    [RoutePrefix("api/employee")]
    public class EMPsController : ApiController
    {
    }
}
```
