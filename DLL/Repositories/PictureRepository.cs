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
    public class PictureRepository : IRepository<Picture>
    {
        private TripContext db;

        public PictureRepository(TripContext context)
        {
            this.db = context;
        }

        public IEnumerable<Picture> GetAll()
        {
            return db.Pictures;
        }

        public Picture Get(long? id)
        {
            return db.Pictures.Find(id);
        }

        public void Create(Picture item)
        {
            db.Pictures.Add(item);
        }

        public void Update(Picture item)
        {
            db.Entry(item).State = EntityState.Modified;
        }

        public IEnumerable<Picture> Find(Func<Picture, Boolean> predicate)
        {
            return db.Pictures.Where(predicate).ToList();
        }

        public void Delete(long? id)
        {
            Picture item = db.Pictures.Find(id);
            if (item != null)
                db.Pictures.Remove(item);
        }
    }
}

