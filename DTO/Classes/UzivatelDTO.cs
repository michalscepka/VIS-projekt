using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Classes
{
	public class UzivatelDTO
	{
		/// <summary>
		/// Id v databázi
		/// </summary>
		public int Id { get; set; }

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
