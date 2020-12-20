using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Classes
{
	public class RezervaceDTO
	{
		/// <summary>
		/// Id začátku rezervace
		/// </summary>
		public int Id { get; set; }

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
		/// Zákazník rezervace
		/// </summary>
		public int ZakaznikId { get; set; }

		/// <summary>
		/// Vozidlo rezervace
		/// </summary>
		public int VozidloId { get; set; }
	}
}
