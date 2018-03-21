using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLL.Entities
{
   public class Picture
    {
        public long id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public virtual Place PlacePictures { get; set; }
        public long? PlaceId { get; set; }
        public virtual Track TrackPictures { get; set; }
        public long? TrackPicturesId { get; set; }
        public virtual CommentPlace CommentPlacePictures { get; set; }
        public long? CommentPlacePicturesId { get; set; }
        public virtual CommentTrack CommentTrackPictures { get; set; }
        public long? CommentTrackPicturesId { get; set; }
    }
}
