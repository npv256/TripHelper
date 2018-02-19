﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using AutoMapper;
using BLL.Interfaces;
using DLL.Entities;
using WEB.Models;

namespace WEB.Controllers
{
    public class HomeController : Controller
    {
        private readonly IService<User> _userService;
        private readonly IService<Place> _placeService;

        public HomeController(IService<User> userService, IService<Place> placeService)
        {
            _userService = userService;
            _placeService = placeService;
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
                Track = "https://raw.githubusercontent.com/npv256/TripHelper/master/WEB/Content/Baigura.kml";
            else
                Track = this.Url.Action("GetReport", "Manage", new { Name = Track }, this.Request.Url.Scheme);
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Place, PlaceViewModels>());
            var mapper = config.CreateMapper();
            var placeList = mapper.Map<List<Place>, List<PlaceViewModels>>(_placeService.GetItemList().ToList());
            ListPlaceUrlTrackModels listPlaceAndTrack = new ListPlaceUrlTrackModels
            {
                PlaceList = placeList,
                UrlTrack = Track
            };
            return View(listPlaceAndTrack);
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