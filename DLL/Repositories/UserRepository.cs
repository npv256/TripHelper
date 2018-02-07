using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DLL.EF;
using DLL.Entities;
using DLL.Interfaces;

namespace DLL.Repositories
{
    public class UserRepository : IRepository<User>
    {
        private TripContext db;

        public UserRepository(TripContext context)
        {
            this.db = context;
        }

        public IEnumerable<User> GetAll()
        {
            return db.Users;
        }

        public User Get(long? id)
        {
            return db.Users.First(user => user.Id == id);
        }

        public void Create(User item)
        {
            db.Users.Add(item);
        }

        public void Update(User item)
        {
            db.Entry(item).State = EntityState.Modified;
        }

        public IEnumerable<User> Find(Func<User, Boolean> predicate)
        {
            return db.Users.Where(predicate).ToList();
        }

        public void Delete(long? id)
        {
            User item = db.Users.Find(id);
            if (item != null)
                db.Users.Remove(item);
        }
    }
}