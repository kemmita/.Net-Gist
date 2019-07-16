1. First create the intermedary class, create a new model titled BookAuthor
```cs
namespace APILegacy.Models
{
    public class BookAuthor
    {
        public int BookId { get; set; }
        public Book Book { get; set; }
        public int AuthorId { get; set; }
        public Author Author { get; set; }
    }
}
```
2. Now in the Book class/Model include the BookAuthor class/model
```cs
namespace APILegacy.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Isbn { get; set; }
        public string Title { get; set; }
        public DateTime DatePublished { get; set; }
        public virtual ICollection<BookAuthor> BookAuthors { get; set; }
    }
}
```
3. Do the same for the Authors model
```cs
namespace APILegacy.Models
{
    public class Author
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual Country Country { get; set; }
        public virtual ICollection<BookAuthor> BookAuthors { get; set; }
    }
}
```
