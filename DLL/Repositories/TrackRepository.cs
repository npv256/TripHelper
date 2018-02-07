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
    public class TrackRepository : IRepository<Track>
    {
        private TripContext db;

        public TrackRepository(TripContext context)
        {
            this.db = context;
        }

        public IEnumerable<Track> GetAll()
        {
            return db.Tracks;
        }

        public Track Get(long? id)
        {
            return db.Tracks.Find(id);
        }

        public void Create(Track item)
        {
            db.Tracks.Add(item);
        }

        public void Update(Track item)
        {
            db.Entry(item).State = EntityState.Modified;
        }

        public IEnumerable<Track> Find(Func<Track, Boolean> predicate)
        {
            return db.Tracks.Where(predicate).ToList();
        }

        public void Delete(long? id)
        {
            Track item = db.Tracks.Find(id);
            if (item != null)
                db.Tracks.Remove(item);
        }
    }
}