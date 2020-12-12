using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Classes
{
	public class ZamestnanecDTO : UzivatelDTO
	{
		/// <summary>
		/// Datum přijetí do zaměstnání
		/// </summary>
		public DateTime DatumNastupu { get; set; }

		/// <summary>
		/// Hodinová mzda zaměstnance
		/// </summary>
		public int HodinovaMzda { get; set; }

		/// <summary>
		/// Id pobočky na které zaměstnanec pracuje
		/// </summary>
		public int PobockaId { get; set; }
	}
}
