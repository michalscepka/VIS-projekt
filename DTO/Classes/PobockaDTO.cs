using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace DTO.Classes
{
	public class PobockaDTO : ISerializable
	{
		/// <summary>
		/// Id pobočky
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

		/// <summary>
		/// Metoda se vola pri serializaci
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("Id", Id);
			info.AddValue("Mesto", Mesto);
			info.AddValue("Ulice", Ulice);
			info.AddValue("Telefon", Telefon);
		}
	}
}
