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
			// Pour les CheckboxCentrale
			List<string> Centrales = model.CheckboxCentrale
				.Where(item => item.IsChecked)
				.Select(item => item.Id)
				.ToList();

			// Pour les CheckboxCritere
			List<string> TypesCommandes;

			if (type == "Verre" && Centrales.Any())
			{
				TypesCommandes = new List<string> { "8" };
			}
			else
			{
				TypesCommandes = model.CheckboxItems
					.Where(item => item.IsChecked)
					.Select(item => item.Id)
					.ToList();
			}

			FiltreManagement filtre = new FiltreManagement();

			if (TypesCommandes.Any() && !Centrales.Any())
			{
				foreach (var item in TypesCommandes)
				{
					filtre.GetOrders(item, "");
				}

			}
			else if (Centrales.Contains("L13") && !TypesCommandes.Any())
			{
				foreach (var item in Centrales)
				{
					filtre.GetOrders("1,4,9", item);
				}

			}
			else if (TypesCommandes.Any() && Centrales.Any())
			{
				foreach (var typeCmd in TypesCommandes)
				{
					if (Centrales.Contains("L13") && Centrales.Contains(" "))
					{

						filtre.GetOrders(typeCmd, "");
					}
					else
					{
						filtre.GetOrders(typeCmd, "L13");

					}
				}
			}

			ViewBag.CheckedItemIds = model.CheckboxItems.Where(item => item.IsChecked).Select(item => item.Id).ToList();
			ViewBag.CheckedItemIdsCentrale = model.CheckboxCentrale.Where(item => item.IsChecked).Select(item => item.Id).ToList();

			model.PositionsNbLignes = filtre.Orders; 
			ViewBag.Type = type;
			TempData["FilteredModel"] = model.PositionsNbLignes;
			return View("FiltreApp", model);
			
		}

        [HttpPost]
        public ActionResult Fichier(MyViewModel model)
        {
				bool PrioriteReserve;
				List<PositionsNbLignes> positions = new List<PositionsNbLignes>();
				int? prioriteReserveMaxLignes;
				int? prioriteReserveMaxQtes;
				List<int> TypePositions = new List<int>();
				List<PositionsNbLignes> positionsListTemp = new List<PositionsNbLignes>();
				List<PositionsNbLignes> positionsListe = TempData["FilteredModel"] as List<PositionsNbLignes>; ;
				positionsListTemp = positionsListe.Where(p => p.TYPETRANSACTION == 4).ToList();
				string ArticleADONIX = "";
				string ArticleNomenclature = "";
				string Article = "";
				int? QuantiteNomEnClatures;
				string priorite = "";
				int? Quantite;



				using (WMSEntities db = new WMSEntities())
				{
					prioriteReserveMaxLignes = db.Passerelle.Select(p => p.PrioriteReserveMaxLignes).SingleOrDefault();
					prioriteReserveMaxQtes = db.Passerelle.Select(p => p.PrioriteReserveMaxQtes).SingleOrDefault();

				}

				foreach (PositionsNbLignes positionsTemp in positionsListTemp)
				{
					string Fusion = "O";
					if (positionsTemp.ORDERS.StartsWith("4"))
					{
						Fusion = "N";

					}
					Article = positionsTemp.NomenclatureArticle;

					PrioriteReserve = false;
					if (prioriteReserveMaxLignes > 0)
					{

						PrioriteReserve = positionsTemp.Nblignes > prioriteReserveMaxLignes;
					}
					if (!PrioriteReserve && prioriteReserveMaxQtes > 0)
					{
						PrioriteReserve = positionsTemp.QUANTITE > prioriteReserveMaxQtes;

					}
					string filePath = Server.MapPath($"../App_Data/{positionsTemp.ORDERS}.txt");
					string data = $"{positionsTemp.TYPETRANSACTION};E;{positionsTemp.ORDERS}_1/1;info entête;{Fusion};Chariot;N;info1;info2;info3;info4;info5";

				
					Quantite = positionsListTemp
					   .Where(p => p.ORDERS == positionsTemp.ORDERS)
					   .Sum(p => p.QUANTITE);

					//Article Fictif
					ArticleADONIX = positionsTemp.ARTICLE;
					ArticleNomenclature = positionsTemp.NomenclatureArticle;
					if (ArticleNomenclature != null)
					{
						Article = ArticleNomenclature;
						QuantiteNomEnClatures = positionsTemp.NomenclatureQuantite;
						Quantite = Quantite * QuantiteNomEnClatures;


					}
					else
					{
						Article = positionsTemp.ARTICLE;
					}

					//Traitement ORTRX
					if (Article == "ORTRX")
					{
						using (WMSEntities db = new WMSEntities())
						{
							var result = db.POSITIONS
							.Where(p => p.ARTICLE.StartsWith("ORTRX") && p.TYPETRANSACTION == 4 && p.ORDERS == positionsTemp.ORDERS && p.NUMEROLIGNE == positionsTemp.NUMEROLIGNE)
							.Select(p => new
							{
								REF5 = p.INFO7
							})
							.FirstOrDefault();
							ArticleADONIX = result.REF5;
						}


					}
					//Traitement OBOX
					if (Article == "OBOX")
					{
						using (WMSEntities db = new WMSEntities())
						{
							var result = db.POSITIONS
							.Where(p => p.ORDERS == positionsTemp.ORDERS && p.NUMEROLIGNE == positionsTemp.NUMEROLIGNE)
							.Select(p => new
							{
								INFO5 = p.INFO5
							})
							.FirstOrDefault();
							ArticleADONIX = result.INFO5;
						}
					}
					//Traitement du rayonnage produit
					if (positionsTemp.ORDERS.StartsWith("4") || positionsTemp.ORDERS.StartsWith("2"))
					{
						using (WMSEntities db = new WMSEntities())
						{
							var result = db.POSITIONS
							.Where(p => p.ORDERS == positionsTemp.ORDERS && p.NUMEROLIGNE == positionsTemp.NUMEROLIGNE)
							.Select(p => new
							{
								INFO5 = p.INFO5
							})
							.FirstOrDefault();
							ArticleADONIX = result.INFO5;
						}
					}
					if (PrioriteReserve == true)
					{
						priorite = "X";
					}

					// Écrire le contenu de la liste dans le fichier texte
					using (StreamWriter writer = new StreamWriter(filePath))
					{
						writer.WriteLine(data);

						foreach (var item in positionsListTemp.Where(p => p.ORDERS.Equals(positionsTemp.ORDERS)))
						{
							string data2 = $"{item.TYPETRANSACTION};L;{item.ORDERS}_1/1;{item.NUMEROLIGNE};{Article};;{item.QUANTITE};N;{ArticleADONIX};{priorite}";

							writer.WriteLine(data2);
						}
					}
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