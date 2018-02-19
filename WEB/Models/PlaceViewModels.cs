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
        public virtual List<CommentPlace> Comments { get; set; }
        [StringLength(500, ErrorMessage = "Значение {0} должно содержать символов не менее: {2}.", MinimumLength = 30)]
        public string Description { get; set; }
        [Range(1, 10, ErrorMessage = "Пожалуйста введите значение от 1 до 10")]
        public double Rating { get; set; }
        public virtual User Author { get; set; }
        [Range(-90, 90, ErrorMessage = "Пожалуйста введите значение от -90.0 до +90.0")]
        public float Latitude { get; set; }
        [Range(-180, 180, ErrorMessage = "Пожалуйста введите значение от -180.0 до +180.0")]
        public float Longitude { get; set; }
        public List<Picture> Pictures { get; set; }

        public PlaceViewModels()
        {
            Comments = new List<CommentPlace>();
            Pictures = new List<Picture>();
        }
    }
}