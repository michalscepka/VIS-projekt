using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.BO
{
	/// <summary>
	/// Reprezentuje pobočku
	/// </summary>
	public class RidicskyPrukaz
	{
		public int Id { get; set; }

		/// <summary>
		/// Číslo řidičského průkazu
		/// </summary>
		public string CisloPrukazu { get; set; }

		/// <summary>
		/// Datum platnosti řidičáku
		/// </summary>
		public DateTime DatumPlatnosti { get; set; }
	}
}
