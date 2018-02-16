using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DLL.EF;
using DLL.Entities;
using DLL.Interfaces;

namespace DLL.Repositories
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private TripContext db;
        private UserRepository _userRepository;
        private TrackRepository _trackRepository;
        private PlaceRepository _placeRepository;
        private PictureRepository _pictureRepository;
        private CommentTrackRepository _commentTrackRepository;
        private CommentPlaceRepository _commentPlaceRepository;

        public EFUnitOfWork(TripContext tripContext)
        {
            db = tripContext;
        }

        public IRepository<User> Users
        {
            get
            {
                if (_userRepository == null)
                    _userRepository = new UserRepository(db);
                return _userRepository;
            }
        }

        public IRepository<Track> Tracks
        {
            get
            {
                if (_trackRepository == null)
                    _trackRepository = new TrackRepository(db);
                return _trackRepository;
            }
        }

        public IRepository<Place> Places
        {
            get
            {
                if (_placeRepository == null)
                    _placeRepository = new PlaceRepository(db);
                return _placeRepository;
            }
        }

        public IRepository<Picture> Pictures
        {
            get
            {
                if (_pictureRepository == null)
                    _pictureRepository = new PictureRepository(db);
                return _pictureRepository;
            }
        }


        public IRepository<CommentPlace> CommentsPlaces
        {
            get
            {
                if (_commentPlaceRepository == null)
                    _commentPlaceRepository = new CommentPlaceRepository(db);
                return _commentPlaceRepository;
            }
        }

        public IRepository<CommentTrack> CommentsTracks
        {
            get
            {
                if (_commentTrackRepository == null)
                    _commentTrackRepository = new CommentTrackRepository(db);
                return _commentTrackRepository;
            }
        }

        public void Save()
        {
            db.SaveChanges();
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
