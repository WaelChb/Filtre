using applicationFiltre.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace applicationFiltre.Services
{
    public class FiltreServices
    {
        public static List<string> Filtre()
        {
            List<string> test = new List<string>();

            try
            {
                using (PP5000Entities db = new PP5000Entities())
                {
                    test = db.POSITIONS.Select(o => o.ORDERS).Take(10).ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Une erreur s'est produite : {ex.Message}");
                throw;
            }

            return test;
        }


        public static List<OrderCount> GetOrderCounts(string checkboxValue , string checkboxValueLa)
        {
            List<OrderCount> orderCounts = new List<OrderCount>();

            try
            {
                using (var db = new PP5000Entities())
                {
                    orderCounts = (from ybt in db.ybPositionsTransporteur2
                                   join o in db.ORDERS on ybt.ORDERS  equals o.ORDERS1
                                   where( o.ORDERS1.StartsWith(checkboxValue)  && ((o.ETAT == 1)|| o.CentraleX3 == (checkboxValueLa))
                                   || ( o.CentraleX3 == (checkboxValueLa) && (o.ORDERS1.StartsWith(checkboxValue) && o.ETAT == 1)))
                                   group ybt by ybt.ORDERS into g
                                   select new OrderCount
                                   {
                                       Orders = g.Key,
                                       NbLigne = g.Count()
                                   }).ToList();

                    //where(o.ORDERS1.StartsWith(checkboxValue) && ((o.ETAT == 1) || (o.CentraleX3 == (checkboxValueLa))


                    //where(o.ORDERS1.StartsWith(checkboxValue) && ((o.ETAT == 1) || (o.CentraleX3 == (checkboxValueLa) && (o.ORDERS1.StartsWith(checkboxValue) && o.ETAT == 1))
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Une erreur s'est produite : {ex.Message}");
                throw;
            }

            return orderCounts;
        }


       
    }




}