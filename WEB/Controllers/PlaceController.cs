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

        public ActionResult Details(String id)
        {
            //var s = User.Identity.IsAuthenticated;
            //var s1 = User.IsInRole("admin");
            //if (!User.Identity.IsAuthenticated || !User.IsInRole("admin"))
            //    return View("");
            if (id != "Nan")
            {
                var somePlace = _placeService.GetItem(Convert.ToInt64(id));
                var config = new MapperConfiguration(cfg => cfg.CreateMap<Place, PlaceViewModels>());
                var mapper = config.CreateMapper();
                var placeViewModel = mapper.Map<Place, PlaceViewModels>(somePlace);
                return PartialView(placeViewModel);
            }
            else
                return View("Error");
        }


        // GET: Place/Create
        public ActionResult Create(long? IdTrack)
        {
            if (!User.Identity.IsAuthenticated)
                return Json(3, JsonRequestBehavior.AllowGet);
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
        public ActionResult Create([Bind(Exclude = "tracks")] PlaceViewModels plVModel,
            IEnumerable<HttpPostedFileBase> fileData)
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                    return Json(3, JsonRequestBehavior.AllowGet);
                if (ModelState.IsValid)
                {
                    var config = new MapperConfiguration(cfg => cfg.CreateMap<PlaceViewModels, Place>());
                    var mapper = config.CreateMapper();
                    var placeModel = mapper.Map<PlaceViewModels, Place>(plVModel);
                    if (plVModel.IdTrack != 0)
                        placeModel.Tracks.Add(_trackService.GetItem(plVModel.IdTrack));
                    if (fileData != null)
                    {
                        fileData = fileData.Where(f => f != null);
                        foreach (var someFile in fileData)
                        {
                            System.IO.FileInfo file = new System.IO.FileInfo(someFile.FileName);
                            if (file.Extension == ".jpeg" || file.Extension == ".jpg" || file.Extension == ".png" ||
                                file.Extension == ".gif" || file.Extension == ".bmp")
                            {
                                string fname = file.Name.Remove((file.Name.Length - file.Extension.Length));
                                fname = fname + System.DateTime.Now.ToString("_ddMMyyhhmmssms") + file.Extension;
                                someFile.SaveAs(Server.MapPath("../Content/Images/" + fname));
                                Picture pic = new Picture
                                {
                                    Name = fname,
                                    Path = Server.MapPath("../Content/Images/" + fname),
                                };
                                placeModel.Pictures.Add(pic);
                            }
                            else
                            {
                                ModelState.AddModelError("WrongFormatImages",
                                    "Разрешенные только следующие форматы изображений : jpeg, jpg, png, gif, bmp");
                                return PartialView(plVModel);
                            }
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("NoImage", "Не выбрано не одного фото");
                        return PartialView(plVModel);
                    }

                    _placeService.Create(placeModel);
                    _placeService.Save();
                    var id = _placeService.GetItemList().FirstOrDefault(track => track == placeModel).Id;
                    return Json(id);
                }
            }
            catch
            {
                return PartialView(plVModel);
            }
            return PartialView(plVModel);
        }

        // GET: Place/Edit/5
        public ActionResult Edit(long id)
        {
            PlaceViewModels plvModels = new PlaceViewModels();
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Place, PlaceViewModels>());
            var mapper = config.CreateMapper();
            plvModels = mapper.Map<Place, PlaceViewModels>(_placeService.GetItem(id));
            return PartialView(plvModels);
        }

        // POST: Place/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( long id, [Bind(Exclude = "tracks")] PlaceViewModels plVModels, IEnumerable<HttpPostedFileBase> fileData)
        {
            try
            {
                if (ModelState.IsValid)
                {                  
                        var config = new MapperConfiguration(cfg => cfg.CreateMap<PlaceViewModels, Place>());
                        var mapper = config.CreateMapper();
                        Place placeDomain = _placeService.GetItem(id);
                        
                        if (plVModels.IdTrack != 0)
                            if(!placeDomain.Tracks.Contains(_trackService.GetItem(plVModels.IdTrack)))
                            placeDomain.Tracks.Add(_trackService.GetItem(plVModels.IdTrack));
                        if (fileData != null)
                        {
                            fileData = fileData.Where(f => f != null);
                            foreach (var someFile in fileData)
                            {
                                System.IO.FileInfo file = new System.IO.FileInfo(someFile.FileName);
                                if (file.Extension == ".jpeg" || file.Extension == ".jpg" || file.Extension == ".png" || file.Extension == ".gif" || file.Extension == ".bmp")
                                {
                                    string fname = file.Name.Remove((file.Name.Length - file.Extension.Length));
                                    fname = fname + System.DateTime.Now.ToString("_ddMMyyhhmmssms") + file.Extension;
                                    someFile.SaveAs(Server.MapPath("~/Content/Images" + fname));
                                    Picture pic = new Picture
                                    {
                                        Name = fname,
                                        Path = Server.MapPath("~/Content/Images" + fname),
                                    };
                                    placeDomain.Pictures.Add(pic);
                                }
                                else
                                {
                                    ModelState.AddModelError("WrongFormatImages", "Разрешенные только следующие форматы изображений : jpeg, jpg, png, gif, bmp");
                                    return PartialView(plVModels);
                                }
                            }
                        }
                        placeDomain.Name = plVModels.Name;
                        placeDomain.Description = plVModels.Description;
                        placeDomain.Latitude = plVModels.Latitude;
                        placeDomain.Longitude = plVModels.Longitude;

                        _placeService.Update(placeDomain);
                        _placeService.Save();
                    var returnId = _placeService.GetItemList().FirstOrDefault(track => track == placeDomain).Id;
                    return Json(returnId);
                }                 
                }
            catch(Exception e)
            {
                return View(plVModels);
            }
            return PartialView(plVModels);
        }

        public ActionResult Delete(long id)
        {
            _placeService.Delete(id);
            _placeService.Save();
            return RedirectToAction("Index");
        }
    }
}
