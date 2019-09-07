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

8. I ran into an issue using stored procedures to insert data, below is the solution.
```sql
CREATE TABLE [dbo].[EMPs] (
    [EMPNO]    INT            IDENTITY (1, 1) NOT NULL,
    [ENAME]    NVARCHAR (MAX) NULL,
    [JOB]      NVARCHAR (MAX) NULL,
    [MGR]      INT            NOT NULL,
    [HIREDATE] DATETIME       NOT NULL,
    [SAL]      INT            NOT NULL,
    [COMM]     INT            NOT NULL,
    [DEPTNO]   INT            NOT NULL,
    CONSTRAINT [PK_dbo.EMPs] PRIMARY KEY CLUSTERED ([EMPNO] ASC)
);

```
```sql
create procedure AddEmployee
@ENAME varchar(50), @JOB varchar(50), @MGR int,  @HIREDATE datetime, @SAL int, @COMM int, @DEPTNO int
as
begin
	insert into EMPs(ENAME, JOB, MGR, HIREDATE, SAL, COMM, DEPTNO) values(@ENAME, @JOB, @MGR, @HIREDATE, @SAL, @COMM, @DEPTNO)
end
```
```cs
        public IHttpActionResult PostEMP(EMP eMP)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            object s;
            using (var db = new KemmitContext())
            {
               s = db.Database.ExecuteSqlCommand("AddEmployee @ENAME, @JOB, @MGR, @HIREDATE, @SAL, @COMM, @DEPTNO",
                    new SqlParameter("ENAME", eMP.ENAME), new SqlParameter("JOB", eMP.JOB), new SqlParameter("MGR", eMP.MGR),
                    new SqlParameter("HIREDATE", eMP.HIREDATE), new SqlParameter("SAL", eMP.SAL), new SqlParameter("COMM", eMP.COMM),                       new SqlParameter("DEPTNO", eMP.DEPTNO));
            }
            return Ok("Gut!" + s);
        }
```
9. If you want to use joins in SQL and retrieve the data in .NET you will need to create model in .NET specifing the tables returned from the join operation
```sql
create proc FetchSumOfEmpSalariesByCity
as
begin
	select SUM(e.SAL) as TotalSalary, d.LOC as Location from EMPS e join DEPTs d on e.DEPTNO = d.DEPTNO group by d.LOC
end
```
10. if we look at the sp above, we will see that our model will need to contain two properties, TotalSalary and Location
```cs
   public class SumOfEmpsSalaryByCity
    {
        [Key]
        public int TotalSalary { get; set; }
        public string Location { get; set; }
    }
```
11. controller action
```cs
        [HttpGet]
        [Route("salary")]
        public IHttpActionResult GetEMPsSal()
        {
            using (var db = new KemmitContext())
            {
                var sp = db.SumOfEmpsSalaryByCities.SqlQuery("FetchSumOfEmpSalariesByCity");
                return Ok(sp.ToList());
            }
        }
```
