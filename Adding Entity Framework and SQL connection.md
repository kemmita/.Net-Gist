  1. Go into NuGet PM and add Entity
  2. Create a Model in the Models directory
  ```cs
      namespace APIUdey.Models
    {
        public class Contractors
        {
            public int Id {get; set;}
            public string Name {get; set;}
            public string Weapon {get; set;}
            public double Rate {get; set;}

        }
    }
  ```
3. In the Models directory create a new class titled ApplicationDBContext
```cs
namespace APIUdey.Models
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Contractor> Contractors { get; set; }
    }
}
```
4. To run the migratrions run and the db will know be created!
```
enable-migrations
```
```
add-migration 'Initial Migration'
```
