using BusinessLayer.BO;
using BusinessLayer.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PresentationLayer
{
	public class RezervaceHelper
	{
		private static readonly object m_LockObj = new object();
		private static RezervaceHelper m_Instance;

		public static RezervaceHelper Instance
		{
			get
			{
				lock (m_LockObj)
				{
					return m_Instance ??= new RezervaceHelper();
				}
			}
		}

		private RezervaceHelper()
		{

		}

		/// <summary>
		/// Vrátí rezervace patřící zákazníkovi
		/// </summary>
		/// <param name="zakaznikId">ID zákazníka</param>
		/// <returns>List rezervací</returns>
		public IEnumerable<Rezervace> GetRezervace(int zakaznikId)
		{
			IEnumerable<Rezervace> rezervaceList = SpravaRezervaci.Instance.SeznamRezervaci;
			foreach (Rezervace rezervace in rezervaceList)
			{
				rezervace.Vozidlo = SpravaVozidel.Instance.FindVozidlo(rezervace.Vozidlo.Id);
				rezervace.Vozidlo.Pobocka = SpravaPobocek.Instance.FindPobocka(rezervace.Vozidlo.Pobocka.Id);
				rezervace.Zakaznik = SpravaZakazniku.Instance.FindZakaznik(zakaznikId);
			}
			return rezervaceList;
		}

		/// <summary>
		/// Zjistí jestli může vytvořit rezervaci
		/// </summary>
		/// <param name="newRezervace">Nová rezervace</param>
		/// <returns>TRUE - může vytvořit rezervaci, FALSE - nemůže</returns>
		public bool CanCreateReservation(Rezervace newRezervace)
		{
			IEnumerable<Rezervace> rezervaceProZvoleneVozidlo = SpravaRezervaci.Instance.SeznamRezervaci.Where(x => x.Vozidlo.Id == newRezervace.Vozidlo.Id).ToList();

			foreach (Rezervace rezervace in rezervaceProZvoleneVozidlo)
				if (!(newRezervace.DatumKonceRezervace < rezervace.DatumZacatkuRezervace || newRezervace.DatumZacatkuRezervace > rezervace.DatumKonceRezervace))
					return false;

			return true;
		}

		/// <summary>
		/// Zjistí jestli může prodloužit rezervaci
		/// </summary>
		/// <param name="selectedRezervace">Vybraná rezervace</param>
		/// <returns>TRUE - může prodloužit rezervaci, FALSE - nemůže</returns>
		public bool CanExtendReservation(Rezervace selectedRezervace)
		{
			IEnumerable<Rezervace> rezervaceProZvoleneVozidlo = SpravaRezervaci.Instance.SeznamRezervaci.Where(x => x.Vozidlo.Id == selectedRezervace.Vozidlo.Id).ToList();

			foreach(Rezervace rezervace in rezervaceProZvoleneVozidlo)
				if(rezervace.Id != selectedRezervace.Id)
					if (!(selectedRezervace.DatumKonceRezervace < rezervace.DatumZacatkuRezervace || selectedRezervace.DatumZacatkuRezervace > rezervace.DatumKonceRezervace))
						return false;

			return true;
		}
	}
}
