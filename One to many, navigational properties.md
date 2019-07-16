1. We have a situationn where we have an Author class and a Country class, the country can have many authors and the author can have one country. 
```cs
namespace APILegacy.Models
{
    public class Author
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual Country Country { get; set; }
    }
}
```
2. Below is the country class that will hold a collection of Authors.
```cs
namespace APILegacy.Models
{
    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Author> Authors { get; set; }
    }
}
```
