using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using BLL.Interfaces;
using DLL.Entities;
using DLL.Interfaces;

namespace BLL.Services
{
    public class UserService : IService<User>
    {

        private readonly IUnitOfWork _db;

        public UserService(IUnitOfWork uof)
        {
            _db = uof;
        }

        public IEnumerable<User> GetItemList(Func<User, Boolean> predicate)
        {
            return _db.Users.Find(predicate);
        }

        public IEnumerable<User> GetItemList()
        {
            return _db.Users.GetAll();
        }

        public User GetItem(long? id)
        {
            return _db.Users.Get(id);
        }

        public void Create(User item)
        {   
            var user = item;
            user.Role = "user";
            _db.Users.Create(user);
        }

        public void Delete(long? s)
        {
            _db.Users.Delete(s);
        }

        public void Update(User item)
        {
            _db.Users.Update(item);
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        public void Save()
        {
            _db.Save();
        }
    }
}