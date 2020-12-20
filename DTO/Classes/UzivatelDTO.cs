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
