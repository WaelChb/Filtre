using applicationFiltre.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using applicationFiltre.Controllers;
using System.Web.UI.WebControls;

namespace applicationFiltre.Services
{
    public class FiltreServices
    {
		public static List<GetOrders_Result> GetOrders(string typeCommande, string centrale)
		{
			List<GetOrders_Result> orders = new List<GetOrders_Result>();

			if (!string.IsNullOrEmpty(typeCommande))
			{
				using (WMSEntities contexte = new WMSEntities())
				{
					orders = contexte.GetOrders(typeCommande, centrale).ToList();
				}
			}
			return orders;
		}

		public static void OrderCancel(string inputAnnule)
        {
			ORDERS annule = new ORDERS();
            try
            {
                using (WMSEntities db = new WMSEntities())
                {
                    annule = db.ORDERS.Where(p => p.ORDERS1 == inputAnnule).SingleOrDefault();
                    annule.ETAT = 999;
                    db.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
		}
	}
}