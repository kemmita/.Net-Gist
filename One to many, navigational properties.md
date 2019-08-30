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
