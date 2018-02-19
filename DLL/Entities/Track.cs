using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLL.Entities
{
    public class Track
    {
        public long? Id { get; set; }
        public string Name { get; set; }
        public virtual List<Place> Places { get; set; }
        public string Description { get; set; }
        public virtual List<CommentTrack> Comments { get; set; }
        public float Rating { get; set; }
        public virtual long? AuthorId { get; set; }
        public virtual User Author { get; set; }
        public virtual string TrackKml { get; set; }
        public virtual List<User> PartyUsers { get; set; }
        public List<Picture> Pictures { get; set; }

        public Track()
        {
            Places = new List<Place>();
            Comments = new List<CommentTrack>();
            PartyUsers = new List<User>();
            Pictures = new List<Picture>();
        }
    }
}
