1. Create Generic Interface
```cs
  public interface IGeneric<T> where T : class
    {
        IList<T> GetAll();
        T GetById(int id);
        void AddElement(T obj);
    }
```
2. Implement Generic Repo
```cs
    public class GenericRepository<T> : IGeneric<T> where T : class
    {
        private readonly KemmitContext _db;
        private DbSet<T> _table;
        public GenericRepository()
        {
            _db = new KemmitContext();
            _table = _db.Set<T>();
            _db.Configuration.ProxyCreationEnabled = false;
        }
        public IList<T> GetAll()
        {
            return _table.ToList();
        }

        public T GetById(int id)
        {
            return _table.Find(id);
        }

        public void AddElement(T obj)
        {
            _table.Add(obj);
            _db.SaveChanges();
        }
    }
```
3. Now in a non generic interface, we can inherit from the generic interface as to not waste time with writing crud methods.
```cs
  public interface ITeacher : IGeneric<Teacher>
    {
        IList<Student> GetStudentsOfTeacher(int teacherId);
    }
```
4. Now let us implement the ITeacher interface
```cs
    public class TeacherRepository : ITeacher
    {
        private readonly KemmitContext _db;
        public TeacherRepository()
        {
            _db = new KemmitContext();
            _db.Configuration.ProxyCreationEnabled = false;
        }

        public IList<Student> GetStudentsOfTeacher(int teacherId)
        {
            return _db.TeacherStudents.Where(ts => ts.TeacherId == teacherId).Select(s => s.Student).ToList();
        }

        public IList<Teacher> GetAll()
        {
            return _db.Teachers.ToList();
        }

        public Teacher GetById(int id)
        {
            return _db.Teachers.FirstOrDefault(t => t.Id == id);
        }

        public void AddElement(Teacher obj)
        {
            _db.Teachers.Add(obj);
            _db.SaveChanges();
        }
    }
```
5. Now lets use the teacher repo for our teacher controller.
```cs
    [RoutePrefix("api/Teacher")]
    public class TeachersController : ApiController
    {
        private readonly ITeacher _teacher;

        public TeachersController(ITeacher teacher)
        {
            _teacher = teacher;
        }

        [HttpGet]
        [Route("")]
        public IList<Teacher> GetTeachers()
        {
            return _teacher.GetAll();
        }

        // GET: api/Teachers/5
        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult GetTeacher(int id)
        {
            return Ok(_teacher.GetById(id));
        }

        // POST: api/Teachers
        [HttpPost]
        [Route("")]
        public IHttpActionResult PostTeacher([FromBody]Teacher teacher)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _teacher.AddElement(teacher);

            return Ok("Alles Geht Gut!");
        }

    }
```
