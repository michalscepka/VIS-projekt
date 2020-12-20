using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.BO
{
	/// <summary>
	/// Třída pro udržování informací o zákaznících IS
	/// </summary>
	public class Zakaznik : Uzivatel
	{
		/// <summary>
		/// Login zákazníka
		/// </summary>
		public string Login { get; set; }

		/// <summary>
		/// Heslo zákazníka
		/// </summary>
		public string Heslo { get; set; }

		/// <summary>
		/// Číslo platební karty zákazníka
		/// </summary>
		public string CisloPlatebniKarty { get; set; }

		/// <summary>
		/// Řidičský průkaz zákazníka
		/// </summary>
		public RidicskyPrukaz RidicskyPrukaz { get; set; }
	}
}
