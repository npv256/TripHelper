using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WEB.Controllers
{
    public class GetKmlController : Controller
    {
        // GET: GetKml
        public ActionResult Index(String name)
        {
            if (name == null)
                name = "Baigura";
            var kml = System.IO.File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath("/Content/"+name+".kml"));
            return Content(kml);
        }
    }
}