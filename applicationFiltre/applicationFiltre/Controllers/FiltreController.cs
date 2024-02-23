using applicationFiltre.Models;
using applicationFiltre.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;


namespace applicationFiltre.Controllers
{
    public class FiltreController : Controller
    {
        // GET: Filtre


        public ActionResult FiltreApp(string type)
        {
            Session["test"] = "test";
            ViewBag.Type = type;
            ViewResult result = View();

            List<OrderCount> model = new List<OrderCount>();

            if (type == "Lentille")
            {
                return View( model);
            }
            else if (type == "Verre")
            {
                return View( model);
            }

            return result;

            

        }


        //---------------------------------------------------------------------------------------------------------


        [HttpPost]
        public ActionResult GetCheckboxValue( string type, bool? checkbox1, bool? checkbox2, bool? checkbox3,bool? checkboxLafayette,bool? checkboxAutre, string checkBoxValue1, string checkBoxValue2,string checkBoxValue3, string checkboxLafayetteValue, string checkboxAutreValue)
        {
            
            // Appeler le service pour obtenir les données
          

            var resultatsFiltres = new List<string>();

            // Logique de filtrage des résultats en fonction des cases cochées
            if (checkbox1 == true)
            {
                checkBoxValue1 = "1";
                ViewBag.checkBoxValue1 = checkBoxValue1;
            }

            if (checkbox2 == true)
            {
                // Ajouter la logique de filtrage pour la checkbox 2
                checkBoxValue2 = "4";
                ViewBag.checkBoxValue2 = checkBoxValue2;
            }

            if (checkbox3 == true)
            {
                // Ajouter la logique de filtrage pour la checkbox 3
                checkBoxValue3 = "9";
                ViewBag.checkBoxValue3 = checkBoxValue3;
            }
            if (checkboxLafayette == true)
            {
                // Ajouter la logique de filtrage pour la checkbox 3
                checkboxLafayetteValue = "L13";
                ViewBag.checkboxLafayetteValue = checkboxLafayetteValue;
            }
            if (checkboxAutre == true)
            {
                // Ajouter la logique de filtrage pour la checkbox 3
                checkboxAutreValue = "";
                ViewBag.checkboxAutreValue = checkboxAutreValue;
            }

            List<OrderCount> model = FiltreServices.GetOrderCounts(checkBoxValue1, checkBoxValue2, checkBoxValue3, checkboxLafayetteValue, checkboxAutreValue);

            ViewBag.Type = type;
            // Retourner la vue "filtre" avec les données
            return View("FiltreApp", model);
        }


    }
}