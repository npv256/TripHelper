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
    public class CommentTrackRepository : IRepository<CommentTrack>
    {
        private TripContext db;

        public CommentTrackRepository(TripContext context)
        {
            this.db = context;
        }

        public IEnumerable<CommentTrack> GetAll()
        {
            return db.CommentsTracks;
        }

        public CommentTrack Get(long? id)
        {
            return db.CommentsTracks.Find(id);
        }

        public void Create(CommentTrack item)
        {
            db.CommentsTracks.Add(item);
        }

        public void Update(CommentTrack item)
        {
            db.Entry(item).State = EntityState.Modified;
        }

        public IEnumerable<CommentTrack> Find(Func<CommentTrack, Boolean> predicate)
        {
            return db.CommentsTracks.Where(predicate).ToList();
        }

        public void Delete(long? id)
        {
            CommentTrack item = db.CommentsTracks.Find(id);
            if (item != null)
                db.CommentsTracks.Remove(item);
        }
    }
}
