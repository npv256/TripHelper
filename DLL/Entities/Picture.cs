using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLL.Entities
{
    public class Picture
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public long? IdPlace { get; set; }
        public long? IdTrack { get; set; }
        public long? IdCommentPlace { get; set; }
        public long? IdCommentTrack { get; set; }
    }
}
