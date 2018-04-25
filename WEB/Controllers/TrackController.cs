using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using BLL.Interfaces;
using DLL.Entities;
using Microsoft.Ajax.Utilities;
using Moq;
using WEB.App_Start;
using WEB.Models;

namespace WEB.Controllers
{
    public class TrackController : Controller
    {
        private readonly IService<User> _userService;
        private readonly IService<Track> _trackService;
        private readonly IService<Place> _placeService;

        public TrackController(IService<User> userService, IService<Track> trackService, IService<Place> placeService)
        {
            _userService = userService;
            _trackService = trackService;
            _placeService = placeService;
        }

        public ActionResult SearchPartial(string name = "", string minCountPlace = "0", string maxCountPlace = "999",
            string minRating = "0")
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Track, TrackViewModels>());
            var mapper = config.CreateMapper();
            var selectedList = _trackService.GetItemList().ToList();
            selectedList = selectedList.Where(Track => Track.Name.Contains(name)).ToList();
            selectedList = selectedList.Where(Track => Track.Rating >= float.Parse(minRating)).ToList();
            selectedList = selectedList.Where(track =>
                    track.Places.Count >= int.Parse(minCountPlace) && track.Places.Count <= int.Parse(maxCountPlace))
                .ToList();
            var trackList = mapper.Map<List<Track>, List<TrackViewModels>>(selectedList);
            return PartialView(trackList);
        }

        public ActionResult Search(string name = "")
        {
            return View((Object) name);
        }

        // GET: Track
        public ActionResult Index()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Track, TrackViewModels>());
            var mapper = config.CreateMapper();
            var trackList = mapper.Map<List<Track>, List<TrackViewModels>>(_trackService.GetItemList().ToList());
            return View(trackList);
        }

        public ActionResult IndexList()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Track, TrackViewModels>());
            var mapper = config.CreateMapper();
            var trackList = mapper.Map<List<Track>, List<TrackViewModels>>(_trackService.GetItemList().ToList());
            return View(trackList);
        }

        // GET: Track/Details/5
        public ActionResult Details(long id)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Track, TrackViewModels>());
            var mapper = config.CreateMapper();
            var trackVM = mapper.Map<Track, TrackViewModels>(_trackService.GetItem(id));
            return PartialView(trackVM);
        }

        // GET: Track/Create
        public ActionResult Create()
        {
            if (!User.Identity.IsAuthenticated)
                return Json(3, JsonRequestBehavior.AllowGet);
            return PartialView();
        }

        // POST: Track/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TrackViewModels trackVM, IEnumerable<HttpPostedFileBase> images, HttpPostedFileBase geoFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //   var kmlfile = fileData.Last();
                    if (_trackService.GetItemList().FirstOrDefault(track => track.Name == trackVM.Name) == null)
                    {
                        var config = new MapperConfiguration(cfg => cfg.CreateMap<TrackViewModels, Track>());
                        var mapper = config.CreateMapper();
                        var trackModel = mapper.Map<TrackViewModels, Track>(trackVM);
                        if (geoFile != null)
                        {
                            System.IO.FileInfo file = new System.IO.FileInfo(geoFile.FileName);
                            if (file.Extension == ".gpx" || file.Extension == ".kml")
                            {
                                string fname = file.Name.Remove((file.Name.Length - file.Extension.Length));

                                fname = fname + file.Extension;
                               // fname = fname + System.DateTime.Now.ToString("_ddMMyyhhmmssms") + file.Extension;
                                geoFile.SaveAs(Server.MapPath("../Content/Tracks/" + fname));
                                trackModel.TrackKml = fname;
                            }
                            else
                            {
                                ModelState.AddModelError("WrongFormatGeo",
                                    "Разрешенные только следующие форматы гео-файлов : gpx, kml");
                                return PartialView(trackVM);
                            }

                        }

                        if (images != null)
                        {
                            images = images.Where(f => f != null);
                            foreach (var someFile in images)
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
                                    trackModel.Pictures.Add(pic);
                                }
                                else
                                {
                                    ModelState.AddModelError("WrongFormatImages",
                                        "Разрешенные только следующие форматы изображений : jpeg, jpg, png, gif, bmp");
                                    return PartialView(trackVM);
                                }
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("NoImage", "Не выбрано не одного фото");
                            return PartialView(trackVM);
                        }

                        _trackService.Create(trackModel);
                        _trackService.Save();
                        return RedirectToAction("Edit", "Track", new {id = trackModel.Id});
                    }
                    else
                    {
                        ModelState.AddModelError("DoubleTrack", "Маршрут с таким названием уже существует");
                        return PartialView(trackVM);
                    }
                }
                else
                {
                    ModelState.AddModelError("ModelEror", "");
                    return PartialView(trackVM);
                }
            }
            catch
            {
                return PartialView(trackVM);
            }
            return PartialView(trackVM);
        }

        // GET: Track/Edit/5
        public ActionResult Edit(long id)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Track, TrackViewModels>());
            var mapper = config.CreateMapper();
            var someTrack = _trackService.GetItem(id);
            var trackVM = mapper.Map<Track, TrackViewModels>(someTrack);
            trackVM.AllPlaces = _placeService.GetItemList().ToList();
            trackVM.AllPlaces = trackVM.AllPlaces.Except(trackVM.Places).ToList();
            return View(trackVM);
        }

        public ActionResult CheckPlaceInTrack(long idPlace, string idTrack)
        {
            if(!User.Identity.IsAuthenticated)
                return Json(3, JsonRequestBehavior.AllowGet);
            try
            {
                if (idTrack.Contains('='))
                    idTrack = idTrack.Substring(idTrack.LastIndexOf('=')+1);
                if(idTrack == "")
                    return Json(4, JsonRequestBehavior.AllowGet);
                if (_trackService.GetItemList().FirstOrDefault(track => track.Id == long.Parse(idTrack)) != null)
            {
                if (_trackService.GetItem(long.Parse(idTrack)).Places.FirstOrDefault(place => place.Id == idPlace) != null)
                    return Json(1, JsonRequestBehavior.AllowGet);
                else
                    return Json(0, JsonRequestBehavior.AllowGet);
            }
            return Json(1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult AddPlaceInTrack(long idPlace, long idTrack)
        {
            if (!User.Identity.IsAuthenticated)
                return Json(3, JsonRequestBehavior.AllowGet);
            var track = _trackService.GetItem(idTrack);
            track.Places.Add(_placeService.GetItem(idPlace));
            _trackService.Update(track);
            _trackService.Save();
             return Json(1, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RemovePlaceInTrack(long idPlace, long idTrack)
        {
            if (!User.Identity.IsAuthenticated)
                return Json(3, JsonRequestBehavior.AllowGet);
            var track = _trackService.GetItem(idTrack);
            track.Places.Remove(_placeService.GetItem(idPlace));
            _trackService.Update(track);
            _trackService.Save();
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CheckTracksByPlace(long id)
        {
           var trackList = _trackService.GetItemList().ToList();
            var tracks = trackList.Where(track => track.Places.Exists(place => place.Id == id)).ToList();
            if (tracks.Count == 0)
                return Json(0, JsonRequestBehavior.AllowGet);
            else
                return Json(tracks.Count, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTracksByPlace(long id)
        {
            var tracks = _trackService.GetItemList().ToList().Where(track => track.Places.Exists(place => place.Id == id));
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Track, TrackViewModels>());
            var mapper = config.CreateMapper();
            var trackList = mapper.Map<List<Track>, List<TrackViewModels>>(tracks.ToList());
            return PartialView(trackList);
        }

        // POST: Track/Edit/5
        [HttpPost]
        public ActionResult Edit(long id, TrackViewModels trackVM, IEnumerable<HttpPostedFileBase> fileData, HttpPostedFileBase kmlfile)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Track/Delete/5
        public ActionResult Delete(long? id)
        {
            _trackService.Delete(id);
            _trackService.Save();
            return RedirectToAction("Index", "Track");
        }
    }
}
