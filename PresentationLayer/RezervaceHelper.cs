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

		public bool CanCreateReservation(Rezervace newRezervace)
		{
			IEnumerable<Rezervace> rezervaceProZvoleneVozidlo = SpravaRezervaci.Instance.SeznamRezervaci.Where(x => x.Vozidlo.Id == newRezervace.Vozidlo.Id).ToList();

			foreach (Rezervace rezervace in rezervaceProZvoleneVozidlo)
				if (!(newRezervace.DatumKonceRezervace < rezervace.DatumZacatkuRezervace || newRezervace.DatumZacatkuRezervace > rezervace.DatumKonceRezervace))
					return false;

			return true;
		}

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
