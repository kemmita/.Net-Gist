0. When running strait SQL, it will have a hard time formatiing the json, so go into Global.asax.cs and in Application_Start make sure it appears as below
```cs
    protected void Application_Start()
        {
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            GlobalConfiguration.Configuration.Formatters.Remove(GlobalConfiguration.Configuration.Formatters.XmlFormatter);
            ConfigureWindsor(GlobalConfiguration.Configuration);
            GlobalConfiguration.Configure(c => WebApiConfig.Register(c, _container));

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
```
1. First we will need to create a model that will model our table.
```cs
namespace MVCSProc.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } 
    }
}
```
2. Now to help scaffold our dbcontext and intitial db, we will create an entity controller off the model we created above.
3. You can choose to use the db created for us, or create your own and alter the connections string in Web.config
```cs
<connectionStrings>
    <add name="MVCSProcContext" connectionString="Data Source=(localdb)\MSSQLLocalDB; Initial Catalog=PersonDbTwo; Integrated Security=True; MultipleActiveResultSets=True; AttachDbFilename=|DataDirectory|MVCSProcContext-20190823101123.mdf"
      providerName="System.Data.SqlClient" />
  </connectionStrings>
```
4. Store Procedure
```sql
create procedure GetAllEmployees
as
begin
	select * from Employees
end
```
5. Now in our controller!
```cs
namespace MVCSProc.Controllers
{
    public class EmployeesController : Controller
    {
        private MVCSProcContext db = new MVCSProcContext();

        public ActionResult Index()
        {
            //Using our db context
            using (var context = new MVCSProcContext())
            {
                //Using our context that was generated and the model we created, we will run the stored Procedure.
                var data = context.Employees.SqlQuery("GetAllEmployees");
                return View(data.ToList());
            }
        }
     }
 }
```
6. We will take it a bit further and pass a param to a stored procedure
```sql
create procedure GetAllEmployee @firstName varchar(50)
as
begin
	select * from Employees where Name = @firstName
end
```
7. Below is how you append an sql param to the stored procedure
```cs
       public ActionResult Catch()
        {
            string name = "Jane";
            using (var context = new MVCSProcContext())
            {
                var data = context.Employees.SqlQuery("GetAllEmployee @firstName", new SqlParameter("firstName", name));
                return View(data.ToList());
            }
        }
```
