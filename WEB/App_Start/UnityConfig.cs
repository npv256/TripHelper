using DLL.EF;
using System;
using DLL.Entities;
using DLL.Interfaces;
using DLL.Repositories;
using Unity;
using Unity.Injection;
using Unity.Lifetime;
using Point = System.Drawing.Point;
using BLL.Interfaces;
using BLL.Services;

namespace WEB
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public static class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container =
          new Lazy<IUnityContainer>(() =>
          {
              var container = new UnityContainer();
              RegisterTypes(container);
              return container;
          });

        /// <summary>
        /// Configured Unity Container.
        /// </summary>
        public static IUnityContainer Container => container.Value;
        #endregion

        /// <summary>
        /// Registers the type mappings with the Unity container.
        /// </summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>
        /// There is no need to register concrete types such as controllers or
        /// API controllers (unless you want to change the defaults), as Unity
        /// allows resolving a concrete type even if it was not previously
        /// registered.
        /// </remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            var connectionString =
                    "Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\aspnet-WEB-20180201100745.mdf;";
            container.RegisterType<TripContext>(new HierarchicalLifetimeManager(),
    new InjectionConstructor(connectionString));
            container.RegisterType<IUnitOfWork, EFUnitOfWork>();
            container.RegisterType<IRepository<User>, UserRepository>();
            container.RegisterType<IRepository<Track>, TrackRepository>();
            container.RegisterType<IRepository<Place>, PlaceRepository>();
            container.RegisterType<IRepository<Picture>, PictureRepository>();
            container.RegisterType<IRepository<CommentPlace>, CommentPlaceRepository>();
            container.RegisterType<IRepository<CommentTrack>, CommentTrackRepository>();


            container.RegisterType<IService<User>, UserService>();
            container.RegisterType<IService<Track>, TrackService>();
            container.RegisterType<IService<Place>, PlaceService>();
            container.RegisterType<IService<CommentPlace>, CommentPlaceService>();
            container.RegisterType<IService<CommentTrack>, CommentTrackService>();

            // NOTE: To load from web.config uncomment the line below.
            // Make sure to add a Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            // TODO: Register your type's mappings here.
            // container.RegisterType<IProductRepository, ProductRepository>();
        }
    }
}