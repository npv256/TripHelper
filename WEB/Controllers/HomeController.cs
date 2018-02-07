using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using BLL.Interfaces;
using DLL.Entities;

namespace WEB.Controllers
{
    public class HomeController : Controller
    {
        private readonly IService<User> _userService;

        public HomeController(IService<User> userService)
        {
            _userService = userService;
        }

        public ActionResult ConstructTrack()
        { 
            return View();
        }


        public ActionResult Index(string Track)
        {
            var s = _userService.GetItemList().ToList();
            var s1 = _userService.GetItemList().First().Email;
            var s2 = _userService.GetItemList().Last();
            if (string.IsNullOrEmpty(Track)) 
                Track = "https://raw.githubusercontent.com/npv256/ParseStatsFromLog/master/Baigura.kml";
            else
                Track = this.Url.Action("GetReport", "Manage", new { Name = Track }, this.Request.Url.Scheme);
            return View((object)Track);
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase upload)
        {
            string fileName="Baigura";
            if (upload != null)
            {
                // получаем имя файла
                 fileName = System.IO.Path.GetFileName(upload.FileName);
                if (fileName.Split('.').Last() != "kml")
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Выберите файл с форматом KML!');</script>");
                }
                // сохраняем файл в папку Files в проекте
                upload.SaveAs(Server.MapPath("~/Content/Tracks/" + fileName));
            }
            return RedirectToAction("Index", new {Track = fileName} );
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}