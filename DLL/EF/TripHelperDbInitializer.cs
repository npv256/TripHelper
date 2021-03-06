﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DLL.Entities;

namespace DLL.EF
{
    public class TripHelperDbInitializer : DropCreateDatabaseIfModelChanges<TripContext>
    {
        protected override void Seed(TripContext db)
        {
            db.Database.ExecuteSqlCommand("ALTER TABLE dbo.Pictures ADD CONSTRAINT Places_Pictures FOREIGN KEY (PlaceId) REFERENCES dbo.Places (Id) ON DELETE SET NULL");
            try
            {
                User u1 = new User
                {
                    Id = 1,
                    Email = "1",
                    Password = "1",
                    Role = "admin"
                };
                User u2 = new User
                {
                    Id = 2,
                    Email = "2",
                    Password = "2",
                    Role = "user"
                };
                User u3 = new User
                {
                    Id = 3,
                    Email = "3",
                    Password = "3",
                    Role = "user"
                };
                db.Users.Add(u1);
                db.Users.Add(u2);
                db.Users.Add(u3);
                db.SaveChanges();
                Picture pic1 = new Picture
                {
                    id = 1,
                    Name = "1.png",
                    Path = "1.png",
                };
                db.Pictures.Add(pic1);
                Place p1 = new Place
                {
                    Id = 1,
                    Name = "1",
                    Author = u1,
                    Description = "1",
                    Latitude = 1,
                    Longitude = 1,
                    Rating = 1,
                    Pictures = new List<Picture>() {pic1},
                };
                p1.Pictures.Add(pic1);
                db.Places.Add(p1);
                Track t1 = new Track
                {
                    Id = 1,
                    Name = "1",
                    Description = "1",
                    Rating = 1,
                    Author = u1,
                    TrackKml = "1",
                    Places =new List<Place>() {p1},
                    Pictures =new List<Picture>() { pic1}
                };
                db.Tracks.Add(t1);
                CommentTrack ct1 = new CommentTrack
                {
                    Id = 1,
                    Author = u2,
                    Rating = 1,
                    TrackComment = t1,
                    Assessment = 1,
                    Text = "1"
                };
                db.CommentsTracks.Add(ct1);
                CommentPlace cp1 = new CommentPlace
                {
                    Id = 1,
                    Author = u2,
                    Rating = 1,
                    PlaceComment = p1,
                    Text = "1",
                    Assessment = 1,
                };
                db.CommentsPlaces.Add(cp1);

                db.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception();
            }
        }
    }
}
