using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLL.Entities
{
    public class CommentPlace : Comment
    {
        public long PlaceId { get; set; }
        public virtual Place PlaceComment { get; set; }
    }
}
