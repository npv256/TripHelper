using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DLL.Entities;

namespace DLL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<User> Users { get; }
        IRepository<Place> Places { get; }
        IRepository<Track> Tracks { get; }
        IRepository<CommentPlace> CommentsPlaces { get; }
        IRepository<CommentTrack> CommentsTracks { get; }
        void Save();
    }
}
