using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace DTO.Classes
{
	public class PobockaDTO : ISerializable
	{
		public int Id { get; set; }

		/// <summary>
		/// Adresa pobočky
		/// </summary>
		public string Mesto { get; set; }

		public string Ulice { get; set; }

		/// <summary>
		/// Telefon na pobočku
		/// </summary>
		public string Telefon { get; set; }

		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("Id", Id);
			info.AddValue("Mesto", Mesto);
			info.AddValue("Ulice", Ulice);
			info.AddValue("Telefon", Telefon);
		}
	}
}
