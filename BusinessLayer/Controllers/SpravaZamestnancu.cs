using BusinessLayer.BO;
using DataLayer.TableDataGateways;
using DTO.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace BusinessLayer.Controllers
{
	/// <summary>
	/// Třída zodpovědná za správu zaměstnanců
	/// </summary>
	public class SpravaZamestnancu
	{
        /// <summary>
        /// Privátní statická proměnná udržující vytvořenou instanci třídy
        /// </summary>
        private static SpravaZamestnancu m_Instance;

        /// <summary>
        /// Pomocný objekt sloužící pro zajištění thread safe přístupu při vytváření instance
        /// </summary>
        private static readonly object m_LockObj = new object ();

        /// <summary>
        /// Seznam všech zamestnancu v systému
        /// </summary>
        public List<Zamestnanec> SeznamZamestnancu { get; }

        /// <summary>
        /// Statická vlastnost třídy, přes kterou se přistupuje ke třídě jako singletonu
        /// </summary>
        public static SpravaZamestnancu Instance
        {
            get
            {
                lock (m_LockObj)
                {
                    return m_Instance ??= new SpravaZamestnancu();
                }
            }
        }

        /// <summary>
        /// Privátní konstruktor, třídu nelze vytvořit jinak, než přes přístup na vlastnost Instance
        /// V rámci konstruktoru načte všechny zaměstnance z uložiště
        /// </summary>
        private SpravaZamestnancu()
        {
            SeznamZamestnancu = new List<Zamestnanec>();

			if (ZamestnanecGW.Instance.LoadAll(out List<ZamestnanecDTO> list, out string errMsg))
			{
				if (list != null)
				{
					foreach (ZamestnanecDTO item in list)
					{
						SeznamZamestnancu.Add(new Zamestnanec()
						{
							Id = item.Id,
							Jmeno = item.Jmeno,
							Prijmeni = item.Prijmeni,
							Email = item.Email,
							Telefon = item.Telefon,
							DatumNarozeni = item.DatumNarozeni,
							DatumNastupu = item.DatumNastupu,
							HodinovaMzda = item.HodinovaMzda,
							Pobocka = new Pobocka() { Id = item.PobockaId }
						});
					}
				}
			}
			else
			{
				throw new Exception($"Chyba Uživatelé: Načteni uživatelů z uložiště \n{errMsg}");
			}
		}

		/// <summary>
		/// Vložení nebo aktualizace objektu zaměstnanec v úložišti
		/// </summary>
		/// <param name="zamestnanec"></param>
		/// <returns>True, pokud se insert/update povedl</returns>
		private bool InsertOrUpdate(Zamestnanec zamestnanec)
        {
			ZamestnanecDTO zamestnanecDTO = new ZamestnanecDTO()
			{
				Id = zamestnanec.Id,
				Jmeno = zamestnanec.Jmeno,
				Prijmeni = zamestnanec.Prijmeni,
				Email = zamestnanec.Email,
				Telefon = zamestnanec.Telefon,
				DatumNarozeni = zamestnanec.DatumNarozeni,
				DatumNastupu = zamestnanec.DatumNastupu,
				HodinovaMzda = zamestnanec.HodinovaMzda,
				PobockaId = zamestnanec.Pobocka.Id
			};

			if (ZamestnanecGW.Instance.InsertOrUpdate(zamestnanecDTO, out string errMsg))
			{
				return true;
			}
			else
			{
				throw new DataException($"Nastala chyba při vložení/aktualizaci zamestnance v uložišti\n {errMsg}");
			}
		}

        /// <summary>
        /// Smazání zaměstnance z úložiště
        /// </summary>
        /// <param name="zamestnanec">Objekt, který chceme smazat</param>
        /// <returns>True, pokud se povedlo smazání</returns>
        private bool Delete(Zamestnanec zamestnanec)
        {
			if (ZamestnanecGW.Instance.Delete(zamestnanec.Id, out string errMsg))
			{
				return true;
			}
			else
			{
				throw new DataException($"Nastala chyba při mazání knihy z uložiště\n {errMsg}");
			}
		}

        /// <summary>
        /// Uložení všech zaměstnanců
        /// </summary>
        public void SaveAllData()
        {
			List<ZamestnanecDTO> zamestnanciDTO = new List<ZamestnanecDTO>();
			foreach (Zamestnanec item in SeznamZamestnancu)
			{
				zamestnanciDTO.Add(new ZamestnanecDTO()
                {
					Id = item.Id,
					Jmeno = item.Jmeno,
					Prijmeni = item.Prijmeni,
					Email = item.Email,
					Telefon = item.Telefon,
					DatumNarozeni = item.DatumNarozeni,
					DatumNastupu = item.DatumNastupu,
					HodinovaMzda = item.HodinovaMzda,
					PobockaId = item.Pobocka.Id
				});
			}

			if (!ZamestnanecGW.Instance.SaveAll(zamestnanciDTO, out string errMsg))
			{
				throw new DataException($"Nastala chyba při zápisu knih do uložiště\n {errMsg}");
			}
		}

        /// <summary>
        /// Vyhledani zaměstnance podle jeho ID
        /// </summary>
        /// <param name="id">ID zaměstnance</param>
        /// <returns>Vrací instanci objektu zaměstnanec nebo null pokud se nic nenašlo</returns>
        public Zamestnanec FindZamestnanec(int id)
        {
			//Zaporne ID značí neuložený nový záznam
			if (id < 0)
                return null;

			//Ověříme, že nemáme objekt již v načteném seznamu, pokud ano tak tento objekt vrátíme
			Zamestnanec zamestnanec = SeznamZamestnancu.Find(x => x.Id == id);
            if (zamestnanec != null)
                return zamestnanec;

			//Nebyl nalezen objekt v seznamu, tak zkusíme uložíště
			if (ZamestnanecGW.Instance.Find(id, out ZamestnanecDTO zamestnanecDTO, out string errMsg))
			{
				return new Zamestnanec()
				{
					Id = zamestnanecDTO.Id,
					Jmeno = zamestnanecDTO.Jmeno,
					Prijmeni = zamestnanecDTO.Prijmeni,
					Email = zamestnanecDTO.Email,
					Telefon = zamestnanecDTO.Telefon,
					DatumNarozeni = zamestnanecDTO.DatumNarozeni,
					DatumNastupu = zamestnanecDTO.DatumNastupu,
					HodinovaMzda = zamestnanecDTO.HodinovaMzda,
					Pobocka = new Pobocka() { Id = zamestnanecDTO.PobockaId }
				};
			}
			else
			{
				throw new DataException($"Nastala chyba při vyhledání zaměstnance v uložišti\n {errMsg}");
			}
		}

		/// <summary>
		/// Vloží noveho zaměstnance do seznamu zaměstnanců a současně i do DB
		/// </summary>
		/// <param name="zamestnanec">Objekt zaměstnanec, ktrerý budeme vkládat</param>
		public void AddZamestnanec(Zamestnanec zamestnanec)
		{
			//Vlozeni objektu do uloziste
			if (InsertOrUpdate(zamestnanec))
			{
				//Vlozeni objektu do seznamu
				SeznamZamestnancu.Add(zamestnanec);
			}
		}

		/// <summary>
		/// Aktualizuje zaměstnance v uložišti
		/// </summary>
		/// <param name="zamestnanec">Objekt zaměstnanec, který chceme aktualizovat v uložišti</param>
		public void UpdateZamestnanec(Zamestnanec zamestnanec)
		{
			//Aktualizace v ulozisti
			if (InsertOrUpdate(zamestnanec))
			{
				//Aktualizace v seznamu
				Zamestnanec updatedZamestnanec = SeznamZamestnancu.Find(x => x.Id == zamestnanec.Id);
				updatedZamestnanec.Id = zamestnanec.Id;
				updatedZamestnanec.Jmeno = zamestnanec.Jmeno;
				updatedZamestnanec.Prijmeni = zamestnanec.Prijmeni;
				updatedZamestnanec.Email = zamestnanec.Email;
				updatedZamestnanec.Telefon = zamestnanec.Telefon;
				updatedZamestnanec.DatumNarozeni = zamestnanec.DatumNarozeni;
				updatedZamestnanec.DatumNastupu = zamestnanec.DatumNastupu;
				updatedZamestnanec.HodinovaMzda = zamestnanec.HodinovaMzda;
				updatedZamestnanec.Pobocka.Id = zamestnanec.Pobocka.Id;
			}
		}

		/// <summary>
		/// Smazání zaměstnance z uložiště i ze seznamu zaměstnanců
		/// </summary>
		/// <param name="zamestnanec"></param>
		public void DeleteZamestnanec(Zamestnanec zamestnanec)
		{
			//Smazani z uloziste
			if (Delete(zamestnanec))
			{
				//Smazani ze seznamu
				SeznamZamestnancu.Remove(SeznamZamestnancu.Find(x => x.Id == zamestnanec.Id));
			}
		}
	}
}
