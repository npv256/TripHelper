using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using BLL.Interfaces;
using DLL.Entities;
using WEB.Models;

namespace WEB.Controllers
{
    public class TrackController : Controller
    {
        private readonly IService<User> _userService;
        private readonly IService<Track> _trackService;

        public TrackController(IService<User> userService, IService<Track> trackService)
        {
            _userService = userService;
            _trackService = trackService;
        }
        // GET: Track
        public ActionResult Index()
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

           return PartialView();
        }

        // POST: Track/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TrackViewModels trackVM, IEnumerable<HttpPostedFileBase> fileData, HttpPostedFileBase kmlfile)
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
                        if (kmlfile != null)
                        {
                            string fileName = System.IO.Path.GetFileName(kmlfile.FileName);
                            kmlfile.SaveAs(Server.MapPath("../Content/Tracks/" + fileName));
                            trackModel.TrackKml = fileName;
                        }
                        else
                        {
                            ModelState.AddModelError("", "Выберите файл с GPS трэком");
                        }
                        if (fileData != null)
                        {
                            fileData = fileData.Where(f => f != null);
                            foreach (var file in fileData)
                            {
                                string fileName = System.IO.Path.GetFileName(file.FileName);
                                file.SaveAs(Server.MapPath("../Content/Images/" + fileName));
                                Picture pic = new Picture
                                 {
                                      Name = file.FileName,
                                      Path = Server.MapPath("../Content/Images/" + fileName),
                                 };
                                 trackModel.Pictures.Add(pic);
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("", "Не выбрано не одного фото");
                        }
                        _trackService.Create(trackModel);
                        _trackService.Save();
                        return RedirectToAction("Edit", "Track",  new { id = trackModel.Id });
                    }
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
            var trackVM = mapper.Map<Track, TrackViewModels>(_trackService.GetItem(id));
            return View(trackVM);
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
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Track/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
