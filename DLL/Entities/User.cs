using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLL.Entities
{
    public class User
    {
        public long? Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public virtual List<Track> SavedTracks { get; set; }
        public virtual List<Place> Places { get; set; }
        public virtual List<Track> CreatedTracks { get; set; }

        public User()
        {
            SavedTracks = new List<Track>();
            Places = new List<Place>();
            CreatedTracks =new List<Track>();
        }
    }
}
