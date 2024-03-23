using applicationFiltre.Models;
using applicationFiltre.Services;
using FiltreConsole;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;


namespace applicationFiltre.Controllers
{
    public class FiltreController : Controller
    {
       
        public ActionResult FiltreApp(string type)
        {
			ViewBag.Type = type;
            ViewResult result = View();
			var model = new MyViewModel
			{
				CheckboxItems = new List<CheckboxItem>
			{
				new CheckboxItem { Id = "1", Label = "1", IsChecked = false },
				new CheckboxItem { Id = "4", Label = "4", IsChecked = false },
				new CheckboxItem { Id = "9", Label = "9", IsChecked = false }
			},
				CheckboxCentrale = new List<CheckboxItem>
				{
					new CheckboxItem { Id = "L13", Label = "La Fayette", IsChecked = false },
					new CheckboxItem { Id = " ", Label = "Autre", IsChecked = false },

				}
			};
			return View(model);
		}
		
		[HttpPost]
		public ActionResult GetCheckboxValue(string type, MyViewModel model)
		{	
			//vérifie si elles sont null 
			if (model.CheckboxItems == null)
			{
				model.CheckboxItems = new List<CheckboxItem>
						{
							new CheckboxItem { Id = "1", Label = "1", IsChecked = false },
							new CheckboxItem { Id = "4", Label = "4", IsChecked = false },
							new CheckboxItem { Id = "9", Label = "9", IsChecked = false }
						};
			}
			if (model.CheckboxCentrale == null)
			{
				model.CheckboxCentrale = new List<CheckboxItem>
						{
							new CheckboxItem { Id = "L13", Label = "La Fayette", IsChecked = false },
							new CheckboxItem { Id = " ", Label = "Autre", IsChecked = false },
						};
			}
			// Pour les CheckboxCritere
			List<string> checkedIdsListCritere = model.CheckboxItems
			.Where(item => item.IsChecked)
			.Select(item => item.Id)
			.ToList();

			// Pour les CheckboxCentrale
			List<string> checkedIdsListCentrale = model.CheckboxCentrale
				.Where(item => item.IsChecked)
				.Select(item => item.Id)
				.ToList();

			FiltreManagement filtre = new FiltreManagement();

			if (checkedIdsListCritere.Any() && !checkedIdsListCentrale.Any())
			{
				foreach (var item in checkedIdsListCritere)
				{
					filtre.GetOrders(item, "");
				}

			}
			else if (checkedIdsListCentrale.Contains("L13") && !checkedIdsListCritere.Any())
			{
				foreach (var item in checkedIdsListCentrale)
				{
					filtre.GetOrders("1,4,9", item);
				}

			}
			else if (checkedIdsListCritere.Any() && checkedIdsListCentrale.Any())
			{
				foreach (var item in checkedIdsListCritere)
				{
					if (checkedIdsListCentrale.Contains("L13") && checkedIdsListCentrale.Contains(" "))
					{

					filtre.GetOrders(item, "");
					}
					else
					{
						filtre.GetOrders(item, "L13");

					}

				}

			}
			
			ViewBag.CheckedItemIds = model.CheckboxItems.Where(item => item.IsChecked).Select(item => item.Id).ToList();
			ViewBag.CheckedItemIdsCentrale = model.CheckboxCentrale.Where(item => item.IsChecked).Select(item => item.Id).ToList();
			model.OrderCount = filtre.Orders; 
			ViewBag.Type = type;
			TempData["FilteredModel"] = model.OrderCount;
			return View("FiltreApp", model);
			
		}

        [HttpPost]
        public ActionResult Valider(MyViewModel model)
        {
            model.OrderCount = TempData["FilteredModel"] as List<OrderCount>;
            if (model.OrderCount != null)
            {
                // Chemin du fichier
                string filePath = Server.MapPath("../App_Data/FilteredModel.txt");
                // Écrire le contenu de la liste dans le fichier texte
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    foreach (var orderCount in model.OrderCount)
                    {
                        writer.WriteLine($"{"order"+" "+orderCount.Orders},{"nbligne"+" "+orderCount.NbOrderLine}");
                    }
                }
				ViewBag.NoFilterMessage = "Filtre applique.";
			}
			else
			{
				ViewBag.NoFilterMessage = "Aucun filtre applique.";
			}
			return View("../Home/Index");
        }

		[HttpPost]
		public ActionResult Annule(string type, string inputAnnule)
		{
			ViewBag.Type = type;
			FiltreServices.OrderCancel(inputAnnule);
			return View("../Home/Index");
		}
	}
}