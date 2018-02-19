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
   public class PlaceRepository : IRepository<Place>
    {
        private TripContext db;

        public PlaceRepository(TripContext context)
        {
            this.db = context;
        }

        public IEnumerable<Place> GetAll()
        {
            return db.Places;
        }

        public Place Get(long? id)
        {
            return db.Places.Find(id);
        }

        public void Create(Place item)
        {
            db.Places.Add(item);
        }

        public void Update(Place item)
        {
            db.Entry(item).State = EntityState.Modified;
        }

        public IEnumerable<Place> Find(Func<Place, Boolean> predicate)
        {
            return db.Places.Where(predicate).ToList();
        }

        public void Delete(long? id)
        {
            Place item = db.Places.Find(id);
            if (item != null)
                db.Places.Remove(item);
        }
    }
}

