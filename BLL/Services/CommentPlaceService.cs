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
    public class CommentPlaceService : IService<CommentPlace>
    {

        private readonly IUnitOfWork _db;

        public CommentPlaceService(IUnitOfWork uof)
        {
            _db = uof;
        }

        public IEnumerable<CommentPlace> GetItemList()
        {
            return _db.CommentsPlaces.GetAll();
        }

        public CommentPlace GetItem(long? id)
        {
            return _db.CommentsPlaces.Get(id);
        }

        public void Create(CommentPlace item)
        {
            _db.CommentsPlaces.Create(item);
        }

        public void Delete(long? s)
        {
            _db.CommentsPlaces.Delete(s);
        }

        public void Update(CommentPlace item)
        {
            _db.CommentsPlaces.Update(item);
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
