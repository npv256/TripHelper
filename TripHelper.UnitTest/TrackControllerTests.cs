using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using NUnit.Framework;
using WEB.Controllers;
using BLL.Interfaces;
using DLL.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WEB.Models;


namespace TripHelper.UnitTest
{
    [TestClass]
    public class TrackControllerTests
    {
        [TestMethod]
        public void Create_GetView_ItsOkViewModelIsTrackView()
        {
            var mockTrack = new Mock<IService<Track>>();
            var mockUser = new Mock<IService<User>>();
            TrackController controller = new TrackController(mockUser.Object, mockTrack.Object);
            TrackViewModels expected = new TrackViewModels();

            PartialViewResult result = controller.Create() as PartialViewResult;
            var actual = result.Model;

            Assert.AreEqual(expected, actual);
        }

    }
}
