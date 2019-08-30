1. We have a situationn where we have many users that can belong to one gym. 
```cs
namespace APILegacy.Models
{
        public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }

        //Foreign Key
        public int GymId { get; set; }
    }
}
```
2. Below is the country class that will hold a collection of Authors.
```cs
  public class Gym
    {
        public int Id { get; set; }
        public string Name { get; set; }

        //Many Users
        public IList<User> Users { get; set; }
    }
```
3. Fetch the many users associated with the gym.
```cs
public IList<User> GetUsersFromGym(int gymId)
{
       return _db.Users.Where(g => g.GymId == gymId).ToList();
}
```

4. Another way perhaps better Down below
we want to create a 1 to many relationship for a user with many photos, so first go to models folder and create a new model called 
```cs
User
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DatingApp.API.models
{
    public class User
    {
        public int Id { get; set; }
        public string UserName {get; set;}
        public byte[] PasswordHash {get; set;} 
        public byte[] PasswordSalt {get; set;}
        public string Gender { get; set; }
        public DateTime DateOfBirth {get; set;}
        public string KnownAs { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive {get; set;}
        public string Introduction { get; set; }
        public string LookingFor { get; set; }
        public string Interests { get; set; }
        public string City { get; set; }
        public string Country { get; set; }

        //here we create the 1-to-many relationship, without using a icollection or ienumerable the one to many would not be established
        public ICollection<Photo> Photos { get; set; }

        //we then create a constructor to intialize the collection
        public User (){
            Photos = new Collection<Photo>();
        }
    }
}
```
 we now need to create our photo model/tabel in our models folder

```cs
namespace DatingApp.API.models
{
    public class Photo
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public DateTime DateAdded { get; set; }
        public bool IsMain { get; set; }

        //here we create a user column that references the user table, we then create a UserId to act as a foreign key
        public User User { get; set; }
        public int UserId { get; set; }
    }
}
```
go to our datacontext and add our dbsets
```cs
using DatingApp.API.models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions<DataContext> options): base(options){}
        public DbSet<User> Users{get; set;}
        public DbSet<Photo> Photos{get; set;}
    }
}
```
now run dotnet ef database update if you check your database all columns and FK should be displayed 
 now we will create a method to fetch both the user "ONE" and the photos "MANY"
 ```cs
  public async Task<User> GetUser(int Id)
    {
      var user = await _context.Users.Include(p => p.Photos).FirstOrDefaultAsync(u=>u.Id == Id);

      return user;
    }
```
```cs
    public async Task<IEnumerable<User>> GetUsers()
    {
      var users = await _context.Users.Include(p=> p.Photos).ToListAsync();
      return users;
    }
```
