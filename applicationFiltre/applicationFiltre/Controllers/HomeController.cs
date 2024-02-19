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
                result = View("Filtre", model);
            }
            else if (type == "Verre")
            {
                result = View("Filtre", model);
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

        [HttpPost]
        public ActionResult GetCheckboxValue(string checkboxValue, string type, string checkboxValueLa)
        {
            // Stocker les valeurs des cases à cocher dans la session
            Session["PostedCheckboxValue"] = checkboxValue;
            Session["PostedCheckboxValueLa"] = checkboxValueLa;

            // Appeler le service pour obtenir les données
            List<OrderCount> model = FiltreServices.GetOrderCounts(checkboxValue, checkboxValueLa);
            ViewBag.Type = type;

            // Retourner la vue "filtre" avec les données
            return View("Filtre", model);
        }

        //----------------------------------------------------------------------------------------------




    }
}