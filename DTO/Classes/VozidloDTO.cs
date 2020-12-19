using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Classes
{
	public class VozidloDTO
	{
		/// <summary>
		/// Id v databázi
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Značka vozidla
		/// </summary>
		public string Znacka { get; set; }

		/// <summary>
		/// Model vozidla
		/// </summary>
		public string Model { get; set; }

		/// <summary>
		/// SPZ vozidla
		/// </summary>
		public string SPZ { get; set; }

		/// <summary>
		/// Cena vozidla na jeden den
		/// </summary>
		public int CenaZaDen { get; set; }

		/// <summary>
		/// Pocet dveri vozidla
		/// </summary>
		public int PocetDveri { get; set; }

		/// <summary>
		/// Motor vozidla a jeho vykon
		/// </summary>
		public string Motor { get; set; }

		/// <summary>
		/// Spotřeba vozidla
		/// </summary>
		public double Spotreba { get; set; }

		public string Obrazek { get; set; }

		/// <summary>
		/// Jestli je vozidlo v nabídce pro zákazníky
		/// </summary>
		public bool Aktivni { get; set; }

		/// <summary>
		/// Id pobočky v databázi
		/// </summary>
		public int PobockaId { get; set; }
	}
}
