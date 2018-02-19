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
        public Place PlacePictures { get; set; }
        public long? PlaceId { get; set; }
        public Track TrackPictures { get; set; }
        public long? CommentPlacePicturesId { get; set; }
        public CommentPlace CommentPlacePictures { get; set; }
        public long? CommentTrackPicturesId { get; set; }
        public CommentTrack CommentTrackPictures { get; set; }
    }
}
