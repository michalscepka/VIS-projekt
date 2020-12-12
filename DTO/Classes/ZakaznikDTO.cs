using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Classes
{
	public class ZakaznikDTO : UzivatelDTO
	{
		public string Login { get; set; }
		public string Heslo { get; set; }
		public string CisloPlatebniKarty { get; set; }
		public int RidicskyPrukazId { get; set; }
	}
}
