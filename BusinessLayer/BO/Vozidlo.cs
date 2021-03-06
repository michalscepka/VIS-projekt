﻿using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.BO
{
	/// <summary>
	/// Reprezentuje vozidlo
	/// </summary>
	public class Vozidlo
	{
		/// <summary>
		/// ID vozidla v databázi
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
		/// Počet dveří vozidla
		/// </summary>
		public int PocetDveri { get; set; }

		/// <summary>
		/// Motor vozidla a jeho výkon
		/// </summary>
		public string Motor { get; set; }

		/// <summary>
		/// Spotřeba vozidla
		/// </summary>
		public double Spotreba { get; set; }

		/// <summary>
		/// Obrázek vozidla
		/// </summary>
		public string Obrazek { get; set; }

		/// <summary>
		/// Jestli je vozidlo v nabídce pro zákazníky
		/// </summary>
		public bool Aktivni { get; set; }

		/// <summary>
		/// Pobočka na které je vozidlo dostupné
		/// </summary>
		public Pobocka Pobocka { get; set; }
	}
}
