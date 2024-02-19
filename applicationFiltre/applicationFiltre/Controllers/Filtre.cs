using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using applicationFiltre.Models;
using applicationFiltre.Services;



namespace applicationFiltre.Controllers
{
    public class Filtre : Controller
    {




        protected bool IsValueSelected(string value)
        {
            string postedValue = Session["PostedCheckboxValue"] as string;
            return postedValue == value;
        }

        protected void CheckboxList_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Mettre à jour la session avec les valeurs sélectionnées
            List<string> selectedValues = new List<string>();
            foreach (ListItem item in checkboxList.Items)
            {
                if (item.Selected)
                {
                    selectedValues.Add(item.Value);
                }
            }
            Session["PostedCheckboxValue"] = string.Join(",", selectedValues);
        }



    }
}