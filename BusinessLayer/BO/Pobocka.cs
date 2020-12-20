using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.BO
{
	/// <summary>
	/// Reprezentuje pobočku
	/// </summary>
	public class Pobocka
	{
		/// <summary>
		/// ID pobočky v databázi
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Město pobočky
		/// </summary>
		public string Mesto { get; set; }

		/// <summary>
		/// Ulice pobočky
		/// </summary>
		public string Ulice { get; set; }

		/// <summary>
		/// Telefon na pobočku
		/// </summary>
		public string Telefon { get; set; }
	}
}
