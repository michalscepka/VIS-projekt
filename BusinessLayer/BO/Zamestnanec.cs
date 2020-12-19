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
		public int Id { get; set; }

		/// <summary>
		/// Datum přijetí do zaměstnání
		/// </summary>
		public DateTime DatumNastupu { get; set; }

		/// <summary>
		/// Hodinová mzda zaměstnance
		/// </summary>
		public int HodinovaMzda { get; set; }

		public Pobocka Pobocka { get; set; }
	}
}
