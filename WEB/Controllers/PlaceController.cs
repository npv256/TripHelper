using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using BLL.Interfaces;
using DLL.Entities;
using Microsoft.Ajax.Utilities;
using WEB.Models;

namespace WEB.Controllers
{
    public class PlaceController : Controller
    {
        private readonly IService<User> _userService;
        private readonly IService<Place> _placeService;

        public PlaceController(IService<User> userService, IService<Place> placeService)
        {
            _userService = userService;
            _placeService = placeService;
        }
        // GET: Place
        public ActionResult Index()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Place, PlaceViewModels>());
            var mapper = config.CreateMapper();
            var placeList = mapper.Map<List<Place>, List<PlaceViewModels>>(_placeService.GetItemList().ToList());
            return View(placeList);
        }

        // GET: Place/Details/5
        public ActionResult Details(long id)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Place, PlaceViewModels>());
            var mapper = config.CreateMapper();
            var placeViewModel = mapper.Map<Place, PlaceViewModels>(_placeService.GetItem(id));
            return View(placeViewModel);
        }

        // GET: Place/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Place/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PlaceViewModels plVModel, IEnumerable<HttpPostedFileBase> fileData)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (_placeService.GetItemList().FirstOrDefault(place => place.Name == plVModel.Name) == null)
                    {
                        var config = new MapperConfiguration(cfg => cfg.CreateMap<PlaceViewModels, Place>());
                        var mapper = config.CreateMapper();
                        var placeModel = mapper.Map<PlaceViewModels, Place>(plVModel);

                        if (fileData != null)
                        {
                            foreach (var file in fileData)
                            {
                                    string fileName = System.IO.Path.GetFileName(file.FileName);
                                    file.SaveAs(Server.MapPath("../Content/Images/" + fileName));
                            }
                            placeModel.Pictures = fileData.Select(x => "../../Content/Images/" + System.IO.Path.GetFileName(x.FileName)).ToArray();
                        }

                        _placeService.Create(placeModel);
                        _placeService.Save();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Place with such name is exist");
                    }
                }
            }
            catch
            {
                return View(plVModel);
            }

            return View(plVModel);
        }

        // GET: Place/Edit/5
        public ActionResult Edit(long id)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Place, PlaceViewModels>());
            var mapper = config.CreateMapper();
            var plVModel = mapper.Map<Place, PlaceViewModels>(_placeService.GetItem(id));
            return View(plVModel);
        }

        // POST: Place/Edit/5
        [HttpPost]
        public ActionResult Edit(long id, PlaceViewModels plVModels, IEnumerable<HttpPostedFileBase> fileData)
        {
            try
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<PlaceViewModels, Place>());
                var mapper = config.CreateMapper();
               var placeModel = mapper.Map<PlaceViewModels, Place>(plVModels);
                _placeService.Update(placeModel);
                _placeService.Save();
                return RedirectToAction("Index");
            }
            catch(Exception e)
            {
                return View(plVModels);
            }
        }

        public ActionResult Delete(long id)
        {
            _placeService.Delete(id);
            _placeService.Save();
            return RedirectToAction("Index");
        }
    }
}
