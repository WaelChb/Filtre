using applicationFiltre.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using applicationFiltre.Controllers;

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


        public static List<OrderCount> GetOrderCounts(string checkBoxValue1, string checkBoxValue2, string checkBoxValue3, string checkboxLafayetteValue,string checkboxAutreValue)
        {
            List<OrderCount> orderCounts = new List<OrderCount>();

            try
            {
                using (var db = new PP5000Entities())
                {
                    List<string> checkBoxValues = new List<string> { checkBoxValue1, checkBoxValue2, checkBoxValue3 , checkboxLafayetteValue, checkboxAutreValue };
                    if (checkboxAutreValue == "")
                    {
                        checkboxAutreValue = null;
                    }
                    if (checkboxLafayetteValue == "")
                    {
                        checkboxLafayetteValue = null;
                    }
                    foreach (var value in checkBoxValues)
                    {
                        if (!string.IsNullOrEmpty(value))
                        {

                            //cas 1
                            if ( checkboxLafayetteValue == null && checkboxAutreValue == null)
                            {
                                orderCounts.AddRange(
                                (from ybt in db.ybPositionsTransporteur2
                                 join o in db.ORDERS on ybt.ORDERS equals o.ORDERS1
                                 where ((o.ORDERS1.StartsWith(value) && o.ETAT == 1))
                                 group ybt by ybt.ORDERS into g
                                 select new OrderCount
                                 {
                                     Orders = g.Key,
                                     NbLigne = g.Count()
                                 }).ToList()
                            );

                            }
                            //cas2 
                            if (checkboxLafayetteValue == "L13" && ((checkBoxValue1 == null || checkBoxValue1 == "") && (checkBoxValue2 == null || checkBoxValue2 == "") && (checkBoxValue3 == null || checkBoxValue3 == "" ) ))
                            {
                                orderCounts.AddRange(
                                (from ybt in db.ybPositionsTransporteur2
                                 join o in db.ORDERS on ybt.ORDERS equals o.ORDERS1
                                 where ((o.CentraleX3=="L13" && o.ETAT == 1))
                                 group ybt by ybt.ORDERS into g
                                 select new OrderCount
                                 {
                                     Orders = g.Key,
                                     NbLigne = g.Count()
                                 }).ToList()
                            );

                            }

                            //cas3
                            if ((checkBoxValue1 == "1" || checkBoxValue2 == "4" || checkBoxValue1 == "9") && checkboxLafayetteValue == "L13")
                            {
                                orderCounts.AddRange(
                                (from ybt in db.ybPositionsTransporteur2
                                 join o in db.ORDERS on ybt.ORDERS equals o.ORDERS1
                                 where ((o.ORDERS1.StartsWith(value) && o.ETAT == 1 && o.CentraleX3 == "L13"))
                                 group ybt by ybt.ORDERS into g
                                 select new OrderCount
                                 {
                                     Orders = g.Key,
                                     NbLigne = g.Count()
                                 }).ToList()
                            );

                            }



                        }
                    }



                    //where(o.ORDERS1.StartsWith(checkboxValue) && ((o.ETAT == 1) || (o.CentraleX3 == (checkboxValueLa))


                    //vrai requete 
                    //  o.ORDERS1.StartsWith(checkboxValue)  && ((o.ETAT == 1)|| o.CentraleX3 == (checkboxValueLa)) || (o.CentraleX3 == (checkboxValueLa) && (o.ORDERS1.StartsWith(checkboxValue) && o.ETAT == 1))



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