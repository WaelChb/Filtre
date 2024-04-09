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
		public List<OrderCount> Orders { get; set; }

		public FiltreManagement()
		{
			Orders = new List<OrderCount>();
		}
		/// <summary>
		/// Obtient une liste de commande à l'aide d'un type de commande et/ou d'un code centrale
		/// </summary>
		public void GetOrders(string typeCommande, string centrale)
		{
			if (!string.IsNullOrEmpty(typeCommande))
			{
				string[] forTypeCmd = typeCommande.Split(',');

				foreach (var currentTypeCmd in forTypeCmd)
				{
					List<GetOrders_Result> orders = FiltreServices.GetOrders(currentTypeCmd, centrale);

					if (orders != null)
					{
						string orderNumber = string.Empty;

						foreach (GetOrders_Result order in orders)
						{
							if (string.IsNullOrEmpty(orderNumber) || orderNumber != order.ORDERS)
							{
								orderNumber = order.ORDERS;
								SetOrdersDto(order.ORDERS, orders.Where(o => o.ORDERS.Equals(order.ORDERS)).ToList().Count());
							}
						}
					}
				}
			}
		}

		/// <summary>
		/// Permet l'alimentation de dto
		/// </summary>
		private void SetOrdersDto(string orderNumber, int nbline)
		{
			OrderCount orderDto = new OrderCount
			{
				Orders = orderNumber,
				NbOrderLine = nbline
			};
			Orders.Add(orderDto);
			Orders.OrderBy(o => o.Orders);
		}
	}
}
