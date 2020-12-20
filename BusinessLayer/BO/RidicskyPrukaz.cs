using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.BO
{
	/// <summary>
	/// Reprezentuje řidičský průkaz zákazníka
	/// </summary>
	public class RidicskyPrukaz
	{
		/// <summary>
		/// ID uživatele v databázi
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Datum platnosti řidičského průkazu
		/// </summary>
		public DateTime DatumPlatnosti { get; set; }
	}
}
