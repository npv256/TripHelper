using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLL.Entities
{
    public class Place
    {
        public long? Id { get; set; }
        public string Name { get; set; }
        public virtual List<CommentPlace> Comments { get; set; }
        public string Description { get; set; }
        public double Rating { get; set; }
        public virtual User Author { get; set; }
        public virtual long? AuthorId { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public virtual List<Track> Tracks { get; set; }
        public List<Picture> Pictures { get; set; }

        public Place()
        {
        Tracks = new List<Track>();
        Comments = new List<CommentPlace>();
        Pictures = new List<Picture>();
        }
    }
}
