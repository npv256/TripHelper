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
    public class CommentPlaceRepository : IRepository<CommentPlace>
    {
        private TripContext db;

        public CommentPlaceRepository(TripContext context)
        {
            this.db = context;
        }

        public IEnumerable<CommentPlace> GetAll()
        {
            return db.CommentsPlaces;
        }

        public CommentPlace Get(long? id)
        {
            return db.CommentsPlaces.Find(id);
        }

        public void Create(CommentPlace item)
        {
            db.CommentsPlaces.Add(item);
        }

        public void Update(CommentPlace item)
        {
            db.Entry(item).State = EntityState.Modified;
        }

        public IEnumerable<CommentPlace> Find(Func<CommentPlace, Boolean> predicate)
        {
            return db.CommentsPlaces.Where(predicate).ToList();
        }

        public void Delete(long? id)
        {
            CommentPlace item = db.CommentsPlaces.Find(id);
            if (item != null)
                db.CommentsPlaces.Remove(item);
        }
    }
}
