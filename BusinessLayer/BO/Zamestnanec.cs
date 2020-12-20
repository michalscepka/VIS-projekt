using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.BO
{
	/// <summary>
	/// Třída s vlastnostmi specifickými pro zaměstnance v IS
	/// </summary>
	public class Zamestnanec : Uzivatel
	{
		/// <summary>
		/// Datum nástupu do zaměstnání
		/// </summary>
		public DateTime DatumNastupu { get; set; }

		/// <summary>
		/// Hodinová mzda zaměstnance
		/// </summary>
		public int HodinovaMzda { get; set; }

		/// <summary>
		/// Pobočka na které zaměstnanec pracuje
		/// </summary>
		public Pobocka Pobocka { get; set; }
	}
}
