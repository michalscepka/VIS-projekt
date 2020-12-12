using BusinessLayer.BO;
using DataLayer.TableDataGateways;
using DTO.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace BusinessLayer.Controllers
{
	public class SpravaZakazniku
	{
		/// <summary>
		/// Privátní statická proměnná udržující vytvořenou instanci třídy
		/// </summary>
		private static SpravaZakazniku m_Instance;

		/// <summary>
		/// Pomocný objekt sloužící pro zajištění thread safe přístupu při vytváření instance
		/// </summary>
		private static readonly object m_LockObj = new object();

		/// <summary>
		/// Seznam všech zamestnancu v systému
		/// </summary>
		public List<Zakaznik> SeznamZakazniku { get; }

		/// <summary>
		/// Celkový počet včech zamestnancu v systému
		/// </summary>
		public int CelkovyPocetZakazniku => SeznamZakazniku.Count;

		/// <summary>
		/// Statická vlastnost třídy, přes kterou se přistupuje ke třídě jako singletonu
		/// </summary>
		public static SpravaZakazniku Instance
		{
			get
			{
				lock (m_LockObj)
				{
					return m_Instance ??= new SpravaZakazniku();
				}
			}
		}

		/// <summary>
		/// Privátní konstruktor, třídu nelze vytvořit jinak, než přes přístup na vlastnost Instance
		/// V rámci konstruktoru načte všechny pobocky z uložiště
		/// </summary>
		private SpravaZakazniku()
		{
			SeznamZakazniku = new List<Zakaznik>();

			if (ZakaznikGW.Instance.LoadAll(out List<ZakaznikDTO> list, out string errMsg))
			{
				if (list != null)
				{
					foreach (ZakaznikDTO item in list)
					{
						SeznamZakazniku.Add(new Zakaznik()
						{
							Id = item.Id,
							Jmeno = item.Jmeno,
							Prijmeni = item.Prijmeni,
							Email = item.Email,
							Telefon = item.Telefon,
							DatumNarozeni = item.DatumNarozeni,
							Login = item.Login,
							Heslo = item.Heslo,
							CisloPlatebniKarty = item.CisloPlatebniKarty,
							RidicskyPrukaz = new RidicskyPrukaz() { Id = item.RidicskyPrukazId }
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
		/// Vložení nebo aktualizace objektu zakaznik v uložišti
		/// </summary>
		/// <param name="zakaznik"></param>
		private bool InsertOrUpdate(Zakaznik zakaznik)
		{
			ZakaznikDTO zakaznikDTO = new ZakaznikDTO()
			{
				Id = zakaznik.Id,
				Jmeno = zakaznik.Jmeno,
				Prijmeni = zakaznik.Prijmeni,
				Email = zakaznik.Email,
				Telefon = zakaznik.Telefon,
				DatumNarozeni = zakaznik.DatumNarozeni,
				Login = zakaznik.Login,
				Heslo = zakaznik.Heslo,
				CisloPlatebniKarty = zakaznik.CisloPlatebniKarty,
				RidicskyPrukazId = zakaznik.RidicskyPrukaz.Id
			};

			if (ZakaznikGW.Instance.InsertOrUpdate(zakaznikDTO, out string errMsg))
			{
				return true;
			}
			else
			{
				throw new DataException($"Nastala chyba při vložení/aktualizaci zamestnance v uložišti\n {errMsg}");
			}
		}

		/// <summary>
		/// Smazání zaměstnance z Uložiště
		/// </summary>
		/// <param name="zakaznik">Objekt, který chceme smazat</param>
		/// <returns>True, pokud se povedlo smazání</returns>
		private bool Delete(Zakaznik zakaznik)
		{
			if (ZakaznikGW.Instance.Delete(zakaznik.Id, out string errMsg))
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
			List<ZakaznikDTO> zamestnanciDTO = new List<ZakaznikDTO>();
			foreach (Zakaznik item in SeznamZakazniku)
			{
				zamestnanciDTO.Add(new ZakaznikDTO()
				{
					Id = item.Id,
					Jmeno = item.Jmeno,
					Prijmeni = item.Prijmeni,
					Email = item.Email,
					Telefon = item.Telefon,
					DatumNarozeni = item.DatumNarozeni,
					Login = item.Login,
					Heslo = item.Heslo,
					CisloPlatebniKarty = item.CisloPlatebniKarty,
					RidicskyPrukazId = item.RidicskyPrukaz.Id
				});
			}

			if (!ZakaznikGW.Instance.SaveAll(zamestnanciDTO, out string errMsg))
			{
				throw new DataException($"Nastala chyba při zápisu knih do uložiště\n {errMsg}");
			}
		}

		/// <summary>
		/// Vyhledani zaměstnance podle jeho ID
		/// </summary>
		/// <param name="id">ID zaměstnance</param>
		/// <returns>Vrací instanci objektu zaměstnanec nebo null pokud se nic nenašlo</returns>
		public Zakaznik FindZakaznik(int id)
		{
			//Zaporne ID značí neuložený nový záznam, takže ho asi nevyhledáme podle iD
			if (id < 0)
				return null;

			//Ověříme, že nemáme knihu již v načteném seznamu, pokud ano tak tento objekt vrátíme
			Zakaznik zakaznik = SeznamZakazniku.Find(x => x.Id == id);
			if (zakaznik != null)
				return zakaznik;

			//Nebyl nalezen objekt v seznamu, tak zkusíme uložíště

			if (ZakaznikGW.Instance.Find(id, out ZakaznikDTO zakaznikDTO, out string errMsg))
			{
				return new Zakaznik()
				{
					Id = zakaznikDTO.Id,
					Jmeno = zakaznikDTO.Jmeno,
					Prijmeni = zakaznikDTO.Prijmeni,
					Email = zakaznikDTO.Email,
					Telefon = zakaznikDTO.Telefon,
					DatumNarozeni = zakaznikDTO.DatumNarozeni,
					Login = zakaznikDTO.Login,
					Heslo = zakaznikDTO.Heslo,
					CisloPlatebniKarty = zakaznikDTO.CisloPlatebniKarty,
					RidicskyPrukaz = new RidicskyPrukaz() { Id = zakaznikDTO.RidicskyPrukazId }
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
		/// <param name="zakaznik">Objekt zaměstnanec, ktrerý budeme vkládat</param>
		public void AddZakaznik(Zakaznik zakaznik)
		{
			if (InsertOrUpdate(zakaznik))
			{
				SeznamZakazniku.Add(zakaznik);
			}
		}

		/// <summary>
		/// Aktualizuje zaměstnance v uložišti
		/// </summary>
		/// <param name="zakaznik">Objekt zaměstnanec, který chceme aktualizovat v uložišti</param>
		public void UpdateZakaznik(Zakaznik zakaznik)
		{
			//Aktualizace v ulozisti
			if (InsertOrUpdate(zakaznik))
			{
				//Aktualizovat musime i objekt v seznamu
				Zakaznik updatedZakaznik = SeznamZakazniku.Find(x => x.Id == zakaznik.Id);
				updatedZakaznik.Id = zakaznik.Id;
				updatedZakaznik.Jmeno = zakaznik.Jmeno;
				updatedZakaznik.Prijmeni = zakaznik.Prijmeni;
				updatedZakaznik.Email = zakaznik.Email;
				updatedZakaznik.Telefon = zakaznik.Telefon;
				updatedZakaznik.DatumNarozeni = zakaznik.DatumNarozeni;
				updatedZakaznik.Login = zakaznik.Login;
				updatedZakaznik.Heslo = zakaznik.Heslo;
				updatedZakaznik.CisloPlatebniKarty = zakaznik.CisloPlatebniKarty;
				updatedZakaznik.RidicskyPrukaz.Id = zakaznik.RidicskyPrukaz.Id;
			}
		}

		/// <summary>
		/// Smazání zaměstnance z uložiště i ze seznamu zaměstnanců
		/// </summary>
		/// <param name="zakaznik"></param>
		public void DeleteZakaznik(Zakaznik zakaznik)
		{
			//Mažeme objekt kniha v uložišti
			if (Delete(zakaznik))
			{
				//Musíme smazat i v seznamu knih
				//SeznamZakazniku.Remove(zakaznik);
				SeznamZakazniku.Remove(SeznamZakazniku.Find(x => x.Id == zakaznik.Id));
			}
		}
	}
}
