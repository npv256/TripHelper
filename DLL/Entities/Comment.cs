using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLL.Entities
{
    public abstract class Comment
    {
        public long? Id { get; set; }
        public string Text { get; set; }
        public byte Assessment { get; set; }
        public virtual User Author { get; set; }
        public virtual long? AuthorId { get; set; }
        public float Rating { get; set; }
        public List<Picture> Pictures {get;set;}
    }
}
