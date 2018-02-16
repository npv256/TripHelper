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
    public class PlaceService : IService<Place>
    {

        private readonly IUnitOfWork _db;

        public PlaceService(IUnitOfWork uof)
        {
            _db = uof;
        }

        public IEnumerable<Place> GetItemList()
        {
            return _db.Places.GetAll();
        }

        public Place GetItem(long? id)
        {
            var somePlace = _db.Places.Get(id);
            somePlace.Pictures = (from pic in _db.Pictures.GetAll()
                                 where pic.PlaceId == id
                                 select pic).ToList();
            var s = _db.Pictures.GetAll().ToList();
            return somePlace;
        }

        public void Create(Place item)
        {
            _db.Places.Create(item);
            foreach (var pic in item.Pictures)
                _db.Pictures.Create(pic);
        }

        public void Delete(long? s)
        {
            _db.Places.Delete(s);
        }

        public void Update(Place item)
        {
            _db.Places.Update(item);
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