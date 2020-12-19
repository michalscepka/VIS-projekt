using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.BO
{
	/// <summary>
	/// Reprezentuje pobočku
	/// </summary>
	public class Pobocka
	{
		public int Id { get; set; }

		/// <summary>
		/// Adresa pobočky
		/// </summary>
		public string Mesto { get; set; }

		public string Ulice { get; set; }

		/// <summary>
		/// Telefon na pobočku
		/// </summary>
		public string Telefon { get; set; }
	}
}
