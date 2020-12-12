using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.BO
{
	/// <summary>
	/// Třída pro udržování informací o zákaznících/uživatelích IS
	/// </summary>
	public class Zakaznik : Uzivatel
	{
		public int Id { get; set; }

		/// <summary>
		/// Login uživatele
		/// </summary>
		public string Login { get; set; }

		/// <summary>
		/// Heslo uživatele
		/// </summary>
		public string Heslo { get; set; }

		/// <summary>
		/// Číslo platebni karty uživatele
		/// </summary>
		public string CisloPlatebniKarty { get; set; }

		public RidicskyPrukaz RidicskyPrukaz { get; set; }

		/// <summary>
		/// Konstruktor třídy
		/// </summary>
		public Zakaznik()
		{

		}
	}
}
