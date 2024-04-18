using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace applicationFiltre.Models
{
	public class PositionsNbLignes
	{
		public int Nblignes { get; set; }


		//De la table Nomenclature
		public string NomenclatureArticle { get; set; }
		public int? NomenclatureQuantite { get; set; }


		//de la table Position
		public string ORDERS { get; set; }
		public int NUMEROLIGNE { get; set; }
		public string ARTICLE { get; set; }
		public Nullable<int> QUANTITE { get; set; }
		public int TYPETRANSACTION { get; set; }
		public string INFO1 { get; set; }
		public string INFO2 { get; set; }
		public string INFO3 { get; set; }
		public string INFO4 { get; set; }
		public string INFO5 { get; set; }
		public string INFO6 { get; set; }
		public Nullable<int> ETAT { get; set; }
		public string INFO7 { get; set; }
	}
}