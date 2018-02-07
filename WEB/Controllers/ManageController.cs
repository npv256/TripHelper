using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BLL.Interfaces;
using DLL.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using WEB.Models;

namespace WEB.Controllers
{
    public class ManageController : Controller
    {
        readonly IService<User> _userService;

        public ManageController(IService<User> userService)
        {
            _userService = userService;
        }

        [Authorize]
        public ActionResult Index()
        {
            if (_userService.GetItemList().Select(s => s.Email).Contains(User.Identity.Name))
            {
                var id = _userService.GetItemList().First(s => s.Email == User.Identity.Name).Id;
                return View();
            }

            return View("Error");
        }

        public void GetReport(string Name)
        {
            Response.ContentType = "application/text";
            Response.AddHeader("Content-Disposition", @"filename="+Name);
            Response.TransmitFile(System.Web.HttpContext.Current.Server.MapPath("/Content/Tracks/" + Name ));
        }
    }
}