using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using DLL.Entities;

namespace WEB.Models
{
    public class PlaceViewModels
    {
        [Key]
        public long Id { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "Значение {0} должно содержать символов не менее: {2}.", MinimumLength = 5)]
        public string Name { get; set; }

        public  List<CommentPlace> Comments { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "Значение {0} должно содержать символов не менее: {2}.", MinimumLength = 50)]
        public string Description { get; set; }

        public double Rating { get; set; }

        public virtual User Author { get; set; }

        [Required]
        [Range(-90, 90, ErrorMessage = "Пожалуйста введите значение от -90.0 до +90.0")]
        public float Latitude { get; set; }

        [Required]
        [Range(-180, 180, ErrorMessage = "Пожалуйста введите значение от -180.0 до +180.0")]
        public float Longitude { get; set; }

        public List<Picture> Pictures { get; set; }

        public  List<Track> Tracks { get; set; }

        public long IdTrack { get; set; }

        public PlaceViewModels()
        {
            Comments = new List<CommentPlace>();
            Pictures = new List<Picture>();
            Tracks = new List<Track>();
        }
    }
}