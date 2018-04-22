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
   public class TrackService : IService<Track>
    {

        private readonly IUnitOfWork _db;

        public TrackService(IUnitOfWork uof)
        {
            _db = uof;
        }

        public IEnumerable<Track> GetItemList()
        {
            return _db.Tracks.GetAll();
        }

        public Track GetItem(long? id)
        {
           var s1 = _db.Tracks.GetAll().LastOrDefault();
            var someTrack = _db.Tracks.Get(id);
            if (someTrack != null)
            {
                if (someTrack.Pictures.Count != 0)
                {
                    someTrack.Pictures = (from pic in _db.Pictures.GetAll()
                        where pic.TrackPicturesId == id
                        select pic).ToList();
                    var s = _db.Pictures.GetAll().ToList();
                }

                return someTrack;
            }
            else
                return null;
        }

        public void Create(Track item)
        {
            _db.Tracks.Create(item);
            foreach (var pic in item.Pictures)
                _db.Pictures.Create(pic);
        }

        public void Delete(long? s)
        {
            _db.Tracks.Delete(s);
        }

        public void Update(Track item)
        {
            _db.Tracks.Update(item);
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