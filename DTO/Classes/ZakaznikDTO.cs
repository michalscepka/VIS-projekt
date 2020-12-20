using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Classes
{
	public class ZakaznikDTO : UzivatelDTO
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
		/// Id řidičského průkazu zákazníka
		/// </summary>
		public int RidicskyPrukazId { get; set; }
	}
}
