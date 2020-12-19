using BusinessLayer.BO;
using BusinessLayer.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PresentationLayer
{
	public class VozidlaHelper
	{
        private static readonly object m_LockObj = new object();
        private static VozidlaHelper m_Instance;

        public static VozidlaHelper Instance
        {
            get
            {
                lock (m_LockObj)
                {
                    return m_Instance ??= new VozidlaHelper();
                }
            }
        }

        /// <summary>
        /// Objekt spravy zamestnancu
        /// </summary>
        private VozidlaHelper()
        {

        }

        public IEnumerable<Vozidlo> GetVozidla()
        {
            IEnumerable<Vozidlo> vozidla = SpravaVozidel.Instance.SeznamVozidel;
            foreach (Vozidlo vozidlo in vozidla)
                vozidlo.Pobocka = SpravaPobocek.Instance.FindPobocka(vozidlo.Pobocka.Id);
            return vozidla;
        }

        public Vozidlo GetVozidlo(int id)
		{
            Vozidlo vozidlo = SpravaVozidel.Instance.FindVozidlo(id);
            vozidlo.Pobocka = SpravaPobocek.Instance.FindPobocka(vozidlo.Pobocka.Id);
            return vozidlo;
        }

        public bool IsVehicleFree(Vozidlo vozidlo, DateTime dateStart, DateTime dateEnd)
		{
            IEnumerable<Rezervace> rezervaceProVozidlo = SpravaRezervaci.Instance.SeznamRezervaci.Where(x => x.Vozidlo.Id == vozidlo.Id).ToList();

            foreach (Rezervace rezervace in rezervaceProVozidlo)
                if (!(dateEnd < rezervace.DatumZacatkuRezervace || dateStart > rezervace.DatumKonceRezervace))
                    return false;

            return true;
        }
	}
}
