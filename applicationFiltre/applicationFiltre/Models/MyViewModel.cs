using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace applicationFiltre.Models
{
	public class MyViewModel
	{
		public List<CheckboxItem> CheckboxItems { get; set; }
		public List<CheckboxItem> CheckboxCentrale { get; set; }
		public List<OrderCount> OrderCount { get; set; }
	}
}