using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLL.Entities
{
    public class CommentTrack : Comment
    {
        public long TrackId { get; set; }
        public Track TrackComment { get; set; }
    }
}
