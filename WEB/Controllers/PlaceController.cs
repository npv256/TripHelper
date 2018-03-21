using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using BLL.Interfaces;
using DLL.Entities;
using Microsoft.Ajax.Utilities;
using Telerik.Web.Mvc.Extensions;
using WEB.Models;

namespace WEB.Controllers
{
    public class PlaceController : Controller
    {
        private readonly IService<User> _userService;
        private readonly IService<Track> _trackService;
        private readonly IService<Place> _placeService;

        public PlaceController(IService<User> userService, IService<Place> placeService, IService<Track> trackService)
        {
            _userService = userService;
            _placeService = placeService;
            _trackService = trackService;
        }
        public ActionResult IndexMap()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Place, PlaceViewModels>());
            var mapper = config.CreateMapper();
            var placeList = mapper.Map<List<Place>, List<PlaceViewModels>>(_placeService.GetItemList().ToList());
            return View(placeList);
        }

        // GET: Place
        public ActionResult Index()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Place, PlaceViewModels>());
            var mapper = config.CreateMapper();
            var placeList = mapper.Map<List<Place>, List<PlaceViewModels>>(_placeService.GetItemList().ToList());
            return View(placeList);
        }

        public ActionResult Details(string id)
        {
            var somePlace = _placeService.GetItemList().FirstOrDefault(place => place.Name == id);
            if (somePlace == null)
                somePlace = _placeService.GetItem(Convert.ToInt64(id));
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Place, PlaceViewModels>());
            var mapper = config.CreateMapper();
            var placeViewModel = mapper.Map<Place, PlaceViewModels>(somePlace);
            return PartialView(placeViewModel);
        }


        // GET: Place/Create
        public ActionResult Create(long? IdTrack)
        {
            PlaceViewModels plvModels = new PlaceViewModels();
            if (IdTrack != null && IdTrack is long)
                plvModels.IdTrack = (long)IdTrack;
            else
                plvModels.IdTrack = 0;
            return PartialView(plvModels);
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
                        if (plVModel.IdTrack != 0)
                            placeModel.Tracks.Add(_trackService.GetItem(plVModel.IdTrack));
                        if (fileData != null)
                        {
                            fileData = fileData.Where(f => f != null);
                            foreach (var file in fileData)
                            {
                                string fileName = System.IO.Path.GetFileName(file.FileName);
                                file.SaveAs(Server.MapPath("~/Content/Images/" + fileName));
                                Picture pic = new Picture
                                {
                                    Name = file.FileName,
                                    Path = Server.MapPath("~/Content/Images/" + fileName),
                                };
                                placeModel.Pictures.Add(pic);
                            }

                        }
                        else
                        {
                            ModelState.AddModelError("", "Не выбрано не одного фото");
                            return PartialView(plVModel);
                        }
                        _placeService.Create(placeModel);
                        _placeService.Save();
                        //return PartialView();
                        // return RedirectToAction("Index","Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Место с таким названием уже существует");
                    }
                }
            }
            catch
            {
                return PartialView(plVModel);
            }

            return PartialView();
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
