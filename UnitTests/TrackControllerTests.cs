using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BLL.Interfaces;
using DLL.Entities;
using Moq;
using WEB.Controllers;
using WEB.Models;
using System.Web.ModelBinding;
using BLL.Services;
using DLL.EF;
using DLL.Repositories;

namespace UnitTests
{
    [TestClass]
    public class TrackControllerTests
    {

        private readonly IService<User> _userService;
        private readonly IService<Track> _trackService;

        public TrackControllerTests()
        {
            _trackService = new TrackService(new EFUnitOfWork(new TripContext()));
        }

        private Mock<IService<Track>> _mockTrack;
        private Mock<IService<User>> _mockUser;
        private Mock<IService<Place>> _mockPlace;
        private TrackController _controller;

        private Track track1;
        private Track track2;

        [TestInitialize]
        public void Initialize()
        {
            foreach (var track in _trackService.GetItemList())
            {
                _trackService.Delete(track.Id);
            }
            _mockTrack = new Mock<IService<Track>>();
            _mockUser = new Mock<IService<User>>();
            _controller = new TrackController(_mockUser.Object, _mockTrack.Object, _mockPlace.Object);
            track1 = new Track() {Name = "firstTrack", Description = "FirstTrackDescription",};
            track2 = new Track() {Name = "secondTrack", Description = "secondTrackDescription",};
                _trackService.Create(track1);
                _trackService.Create(track2);
                _trackService.Save();
        }

        [TestMethod]
        public void Details_GetView_CheckRightView()
        {

            _controller = new TrackController(_mockUser.Object, _trackService, _mockPlace.Object);

            var result = _controller.Details(_trackService.GetItemList().FirstOrDefault().Id.Value) as PartialViewResult; 

            Assert.AreEqual("firstTrack", ((TrackViewModels) result.ViewData.Model).Name);
        }

        [TestMethod]
        public void Create_PostData_ItIsOkModel()
        {
            var image1 = new Mock<HttpPostedFileBase>();
            image1.Setup(f => f.ContentLength).Returns(1);
            image1.Setup(f => f.FileName).Returns("myFileName.jpg");
            var image2 = new Mock<HttpPostedFileBase>();
            image2.Setup(f => f.ContentLength).Returns(1);
            image2.Setup(f => f.FileName).Returns("myFileName.jpg");
            List<HttpPostedFileBase> images = new List<HttpPostedFileBase>() {image1.Object, image2.Object};
            TrackViewModels track =
                new TrackViewModels() {Name = "ThisIsTestName", Description = "ThisIsTestDescription"};

            var result = _controller.Create(track, images, null);

            Assert.IsTrue(_controller.ViewData.ModelState.Count == 0);
        }

        [TestMethod]
        public void Create_PostData_CheckWhithoutImages()
        {
            TrackViewModels track =
                new TrackViewModels() {Name = "ThisIsTestName", Description = "ThisIsTestDescription"};

            var result = _controller.Create(track, null, null);

            Assert.IsTrue(_controller.ViewData.ModelState.Count == 1);
            Assert.IsTrue(_controller.ViewData.ModelState.Keys.Contains("NoImage"));
        }

        [TestMethod]
        public void Create_PostData_CheckWrongFormatImages()
        {
            var image1 = new Mock<HttpPostedFileBase>();
            image1.Setup(f => f.ContentLength).Returns(1);
            image1.Setup(f => f.FileName).Returns("myFileName1.asd");
            var image2 = new Mock<HttpPostedFileBase>();
            image2.Setup(f => f.ContentLength).Returns(1);
            image2.Setup(f => f.FileName).Returns("myFileName2.asd");
            List<HttpPostedFileBase> images = new List<HttpPostedFileBase>() {image1.Object, image2.Object};
            TrackViewModels track =
                new TrackViewModels() {Name = "ThisIsTestName", Description = "ThisIsTestDescription"};

            var result = _controller.Create(track, images, null);

            Assert.IsTrue(_controller.ViewData.ModelState.Count == 1);
            Assert.IsTrue(_controller.ViewData.ModelState.Keys.Contains("WrongFormatImages"));
        }

        [TestMethod]
        public void Create_PostData_CheckWrongFormatGeo()
        {
            var image1 = new Mock<HttpPostedFileBase>();
            image1.Setup(f => f.ContentLength).Returns(1);
            image1.Setup(f => f.FileName).Returns("myFileName1.jpg");
            var image2 = new Mock<HttpPostedFileBase>();
            image2.Setup(f => f.ContentLength).Returns(1);
            image2.Setup(f => f.FileName).Returns("myFileName2.jpg");
            List<HttpPostedFileBase> images = new List<HttpPostedFileBase>() {image1.Object, image2.Object};
            var geoFile = new Mock<HttpPostedFileBase>();
            geoFile.Setup(f => f.ContentLength).Returns(1);
            geoFile.Setup(f => f.FileName).Returns("myFileName3.jpg");

            TrackViewModels track =
                new TrackViewModels() {Name = "ThisIsTestName", Description = "ThisIsTestDescription"};

            var result = _controller.Create(track, images, geoFile.Object);

            Assert.IsTrue(_controller.ViewData.ModelState.Count == 1);
            Assert.IsTrue(_controller.ViewData.ModelState.Keys.Contains("WrongFormatGeo"));
        }

        [TestMethod]
        public void Edit_GetView_CheckRightView()
        {
            _controller = new TrackController(_mockUser.Object, _trackService, _mockPlace.Object);

            var result = _controller.Edit(_trackService.GetItemList().FirstOrDefault().Id.Value) as ViewResult; 
            var resultData = (TrackViewModels) result.ViewData.Model;

            Assert.AreEqual(track1.Name, resultData.Name );
        }

        [TestMethod]
        public void Edit_PostData_CheckValidModel()
        {

        }

        [TestMethod]
        public void Delete_ClickDelete_CheckWork()
        {
            _controller = new TrackController(_mockUser.Object, _trackService, _mockPlace.Object);
            var lastTrackBeforeDel = _trackService.GetItemList().LastOrDefault();

            var result = _controller.Delete(lastTrackBeforeDel.Id.Value) as RedirectToRouteResult;

            Assert.AreEqual("Index", result.RouteValues["action"]);
            _trackService.Create(track2);
            _trackService.Save();
        }
    }
}
