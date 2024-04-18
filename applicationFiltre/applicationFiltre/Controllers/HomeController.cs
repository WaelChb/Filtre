//using applicationFiltre.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using applicationFiltre.Models;
using applicationFiltre.Services;


namespace applicationFiltre.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(string type)
        {
            ViewBag.Type = type;

            ViewResult result = View();

            List<OrderCount> model = new List<OrderCount>();

            if (type == "Lentille")
            {
                result = View("FiltreApp", model);
            }
            else if (type == "Verre")
            {
                result = View("FiltreApp", model);
            }

            return result;
        }

        // le reste 
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