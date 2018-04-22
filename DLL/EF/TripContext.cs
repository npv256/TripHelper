using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DLL.Entities;

namespace DLL.EF
{
   public class TripContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Track> Tracks { get; set; }
        public DbSet<Place> Places { get; set; }
        public DbSet<CommentPlace> CommentsPlaces { get; set; }
        public DbSet<CommentTrack> CommentsTracks { get; set; }
        public DbSet<Picture> Pictures { get; set; }

        public TripContext()
        {
            Database.SetInitializer(new TripHelperDbInitializer());
        }
        public TripContext(string connectionString)
            : base(connectionString)
        {
            Database.SetInitializer(new TripHelperDbInitializer());
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(c => c.SavedTracks)
                .WithMany(s => s.PartyUsers)
                .Map(t => t.MapLeftKey("TrackId")
                    .MapRightKey("UserId")
                    .ToTable("UserSavedTracks"));

            modelBuilder.Entity<Track>().HasMany(c => c.Places)
                .WithMany(s => s.Tracks)
                .Map(t => t.MapLeftKey("PlaceId")
                    .MapRightKey("TrackId")
                    .ToTable("TrackPlaces"));

            
        }


    }
}