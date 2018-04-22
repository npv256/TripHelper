using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DLL.Entities;

namespace WEB.Models
{
    public class CommentModels
    {
        public long? Id { get; set; }
        public string Text { get; set; }
        public byte Assessment { get; set; }
        public virtual User Author { get; set; }
        public float Rating { get; set; }
        public virtual Picture Picture { get; set; }
        public long TargetId { get; set; }
        public string TypeComment { get; set; }
    }
}