1. At times you willl need to establish a connection with your sql db from a .net app, and that db may already be created
with tables, data, and SPs.
2. After creating the project, ensure that you have EntityFramework installed.
3. right click on your project and select add new item.
4. Under Data, you will find ADO.NET Entity Data Model, this will help you set up the connections and create your
application db context.
5. Look for the .edmx extension in your projects directory to view the application db context created and also the domain
models generated for you.
6. Then accessing your db is as easy as
```cs
public class PostsController : ApiController
    {
        private readonly DatabaseFirstDemoEntities _db;
        public PostsController()
        {
            _db = new DatabaseFirstDemoEntities();
        }
        // GET api/values
        public async Task<IList<Post>> Get()
        {
            return await _db.Posts.ToListAsync();
        }
    }
```
7. If you need to make changes to the DB, simply go the edmx designer and select Update model from db.
