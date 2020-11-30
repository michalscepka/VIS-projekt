using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.BO
{
	/// <summary>
	/// Reprezentuje jednu rezervaci vozidla
	/// </summary>
	public class Rezervace
	{
		/// <summary>
		/// Datum začátku rezervace
		/// </summary>
		public DateTime DatumZacatkuRezervace { get; set; }

		/// <summary>
		/// Datum konce rezervace
		/// </summary>
		public DateTime DatumKonceRezervace { get; set; }

		/// <summary>
		/// Cena rezervace
		/// </summary>
		public int Cena { get; set; }

		/// <summary>
		/// Kauce rezervace
		/// </summary>
		public int Kauce { get; set; }

		/// <summary>
		/// Konstruktor třídy
		/// </summary>
		public Rezervace()
		{

		}
	}
}
