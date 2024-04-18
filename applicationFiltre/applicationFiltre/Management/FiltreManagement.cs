using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using applicationFiltre.Models;
using applicationFiltre.Services;

namespace FiltreConsole
{
	public class FiltreManagement
	{

		public List<PositionsNbLignes> Orders { get; set; }

		public FiltreManagement()
		{
			Orders = new List<PositionsNbLignes>();
		}
		/// <summary>
		/// Obtient une liste de commande à l'aide d'un type de commande et/ou d'un code centrale
		/// </summary>
		public void GetOrders(string typeCommande, string centrale)
		{
			List<GetOrders_Result> orders = new List<GetOrders_Result>();
			if (!string.IsNullOrEmpty(typeCommande))
			{
				string[] forTypeCmd = typeCommande.Split(',');

				foreach (var currentTypeCmd in forTypeCmd)
				{
					orders = FiltreServices.GetOrders(currentTypeCmd, centrale);

					if (orders != null)
					{
						SetPositionsNbLignesDto(orders);
					}

				}
			}
		}

		/// <summary>
		/// Permet l'alimentation de dto
		/// </summary>

		private void SetPositionsNbLignesDto(List<GetOrders_Result> orders)
		{

			foreach (GetOrders_Result order in orders)
			{
				PositionsNbLignes positionsTemp = new PositionsNbLignes();
				positionsTemp.NomenclatureArticle = order.ArticleNom;
				positionsTemp.NomenclatureQuantite = order.QuantiteNom;
				positionsTemp.ARTICLE = order.ARTICLE;
				positionsTemp.ORDERS = order.ORDERS;
				positionsTemp.NUMEROLIGNE = order.NUMEROLIGNE;
				positionsTemp.QUANTITE = order.QUANTITE;
				positionsTemp.TYPETRANSACTION = order.typetransaction;

				positionsTemp.Nblignes = orders.Where(o => o.ORDERS.Equals(order.ORDERS)).Count();
				Orders.Add(positionsTemp);
			}

		}
	}
}
