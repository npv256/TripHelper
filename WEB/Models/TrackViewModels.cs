using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using DLL.Entities;

namespace WEB.Models
{
    public class TrackViewModels
    {
        [Key]
        public long Id { get; set; }
        [Required]     
        public string Name { get; set; }
        public virtual List<Place> Places { get; set; }
        public string Description { get; set; }
        public virtual List<CommentTrack> Comments { get; set; }
        public float Rating { get; set; }
        public virtual User Author { get; set; }
        public virtual string TrackKml { get; set; }
        public List<Picture> Pictures { get; set; }
        public List<Place> AllPlaces { get; set; }

        public TrackViewModels()
        {
            Places = new List<Place>();
            Comments = new List<CommentTrack>();
            Pictures =new List<Picture>();
            AllPlaces = new List<Place>();
        }
    }
}