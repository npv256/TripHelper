using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Mvc;
using BLL.Interfaces;
using DLL.Entities;
using Microsoft.AspNet.Identity;
using WEB.Models;

namespace WEB.Controllers
{
    public class AccountController : Controller
    {
        private readonly IService<User> _userService;

        public AccountController(IService<User> userService)
        {
            _userService = userService;
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            if (_userService.GetItemList().FirstOrDefault(user => user.Email == model.Email && user.Password == model.Password)!=null)
            {
                FormsAuthentication.SetAuthCookie(model.Email, true);
              var s=  FormsAuthentication.FormsCookieName;

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "User with such login and password is not exist");
            }

            return View(model);
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (_userService.GetItemList().FirstOrDefault(user => user.Email==model.Email)==null)
                {
                    User someUser = new User
                    {
                        Email = model.Email,
                        Password = model.Password
                    };
                    _userService.Create(someUser);
                    _userService.Save();
                    FormsAuthentication.SetAuthCookie(model.Email, true);
                    return RedirectToAction("Index", "Home");
                }
                else
                ModelState.AddModelError("", "Пользователь с таким Email уже существует");
            }
            return View(model);
        }

        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}