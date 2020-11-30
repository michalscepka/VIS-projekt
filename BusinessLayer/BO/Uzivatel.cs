using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.BO
{
	/// <summary>
	/// Bázový kontejner se společnými vlastnostmi pro uživatele IS
	/// </summary>
	public abstract class Uzivatel
	{
		/// <summary>
		/// Jméno osoby
		/// </summary>
		public string Jmeno { get; set; }

		/// <summary>
		/// Příjmení osoby
		/// </summary>
		public string Prijmeni { get; set; }

		/// <summary>
		/// Email osoby
		/// </summary>
		public string Email { get; set; }

		/// <summary>
		/// Telefon osoby
		/// </summary>
		public string Telefon { get; set; }

		/// <summary>
		/// Datum narození osoby
		/// </summary>
		public DateTime DatumNarozeni { get; set; }
	}
}
