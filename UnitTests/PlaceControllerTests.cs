using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BLL.Interfaces;
using BLL.Services;
using DLL.EF;
using DLL.Entities;
using DLL.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WEB.Controllers;
using WEB.Models;

namespace UnitTests
{
    [TestClass]
    public class PlaceControllerTests
    {
        private readonly IService<User> _userService;
        private readonly IService<Place> _placeService;
        private readonly IService<Track> _trackService;

        private Mock<IService<Track>> _mockTrack;
        private Mock<IService<User>> _mockUser;
        private PlaceController _controller;

        private Place place1 = null;
        private Place place2 = null;
        private Place place3 = null;


        public PlaceControllerTests()
        {
            _placeService = new PlaceService(new EFUnitOfWork(new TripContext()));
        }

         [TestInitialize]
        public void Initialize()
         {
            foreach (var place in _placeService.GetItemList())
            {
                _placeService.Delete(place.Id);
            }
             _placeService.Save();
            _mockTrack = new Mock<IService<Track>>();
            _mockUser = new Mock<IService<User>>();
            _controller = new PlaceController(_mockUser.Object, _placeService, _mockTrack.Object);
            place1 = new Place() {Name = "firstPlaceName", Description = "fisrstPlaceDescription"}; 
            place2 = new Place() {Name = "secondPlaceName", Description =  "secondPlaceDescription"};
         //   place3 = new Place() { Name = "thridPlaceName", Description = "thridPlaceDescription", };
            _placeService.Create(place1);
           _placeService.Create(place2);
           _placeService.Save();
           
        }

        [TestMethod]
        public void Index_GetAllPlaces_ItIsOkModelView()
        {
            var result = _controller.Index() as ViewResult;
            var resultData = (List<PlaceViewModels>) result.Model;
            int placeListCount = 2; // place1 and place 2
            Assert.AreEqual(placeListCount, resultData.Count);
        }

        [TestMethod]
        public void Details_GetView_CheckRightView()
        {

            var result = _controller.Details(_placeService.GetItemList().FirstOrDefault().Id.ToString()) as PartialViewResult;

            Assert.AreEqual("firstPlaceName", ((PlaceViewModels)result.ViewData.Model).Name);
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
            List<HttpPostedFileBase> images = new List<HttpPostedFileBase>() { image1.Object, image2.Object };
            PlaceViewModels place =
                new PlaceViewModels() { Name = "ThisIsTestName", Description = "ThisIsTestDescription" };

            var result = _controller.Create(place, images);

            Assert.IsTrue(_controller.ViewData.ModelState.Count == 0);
        }

        [TestMethod]
        public void Create_PostData_CheckWhithoutImages()
        {
            PlaceViewModels place =
                new PlaceViewModels() { Name = "ThisIsTestName", Description = "ThisIsTestDescription" };

            var result = _controller.Create(place, null);

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
            List<HttpPostedFileBase> images = new List<HttpPostedFileBase>() { image1.Object, image2.Object };
            PlaceViewModels place =
                new PlaceViewModels() { Name = "ThisIsTestName", Description = "ThisIsTestDescription" };

            var result = _controller.Create(place, images);

            Assert.IsTrue(_controller.ViewData.ModelState.Count == 1);
            Assert.IsTrue(_controller.ViewData.ModelState.Keys.Contains("WrongFormatImages"));
        }

        [TestMethod]
        public void Edit_GetView_CheckRightView()
        {

            var result = _controller.Edit(_placeService.GetItemList().FirstOrDefault().Id.Value) as ViewResult;
            var resultData = (PlaceViewModels)result.ViewData.Model;

            Assert.AreEqual(place1.Name, resultData.Name);
        }

        [TestMethod]
        public void Edit_PostData_CheckValidModel()
        {

        }

        [TestMethod]
        public void Delete_ClickDelete_CheckWork()
        {
            var lastPlaceBeforeDel = _placeService.GetItemList().LastOrDefault();

            var result = _controller.Delete(lastPlaceBeforeDel.Id.Value) as RedirectToRouteResult;

            Assert.AreEqual("Index", result.RouteValues["action"]);
            _placeService.Create(place2);
            _placeService.Save();
        }



    }
}
