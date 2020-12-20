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
		/// ID uživatele v databázi
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Jméno uživatele
		/// </summary>
		public string Jmeno { get; set; }

		/// <summary>
		/// Příjmení uživatele
		/// </summary>
		public string Prijmeni { get; set; }

		/// <summary>
		/// Email uživatele
		/// </summary>
		public string Email { get; set; }

		/// <summary>
		/// Telefon uživatele
		/// </summary>
		public string Telefon { get; set; }

		/// <summary>
		/// Datum narození uživatele
		/// </summary>
		public DateTime DatumNarozeni { get; set; }
	}
}
