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
            var placeList = mapper.Map<IEnumerable<Place>, IEnumerable<PlaceViewModels>>(_placeService.GetItemList());
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
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Place/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Place/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
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

        // GET: Place/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Place/Delete/5
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
