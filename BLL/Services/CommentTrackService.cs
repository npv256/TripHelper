using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Interfaces;
using DLL.Entities;
using DLL.Interfaces;

namespace BLL.Services
{
    public class CommentTrackService : IService<CommentTrack>
    {

        private readonly IUnitOfWork _db;

        public CommentTrackService(IUnitOfWork uof)
        {
            _db = uof;
        }

        public IEnumerable<CommentTrack> GetItemList()
        {
            return _db.CommentsTracks.GetAll();
        }

        public CommentTrack GetItem(long? id)
        {
            return _db.CommentsTracks.Get(id);
        }

        public void Create(CommentTrack item)
        {
            _db.CommentsTracks.Create(item);
        }

        public void Delete(long? s)
        {
            _db.CommentsTracks.Delete(s);
        }

        public void Update(CommentTrack item)
        {
            _db.CommentsTracks.Update(item);
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
