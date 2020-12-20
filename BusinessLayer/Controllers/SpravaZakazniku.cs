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
	/// Třída zodpovědná za správu zákazníků
	/// </summary>
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
		/// V rámci konstruktoru načte všechny zákazníky z uložiště
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
		/// Vložení nebo aktualizace objektu zákazník v úložišti
		/// </summary>
		/// <param name="zakaznik"></param>
		/// <returns>True, pokud se insert/update povedl</returns>
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
		/// Smazání zákazníka z úložiště
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
		/// Uložení všech zákazníků
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
		/// Vyhledání zákazníka podle jeho ID
		/// </summary>
		/// <param name="id">ID zákazníka</param>
		/// <returns>Vrací instanci objektu zákazník nebo null pokud se nic nenašlo</returns>
		public Zakaznik FindZakaznik(int id)
		{
			//Zaporne ID značí neuložený nový záznam
			if (id < 0)
				return null;

			//Ověříme, že nemáme objekt již v načteném seznamu, pokud ano tak tento objekt vrátíme
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
		/// Vloží nového zákazníka do seznamu zákazníků a současně i do DB
		/// </summary>
		/// <param name="zakaznik">Objekt zákazník, ktrerý budeme vkládat</param>
		public void AddZakaznik(Zakaznik zakaznik)
		{
			//Vlozeni objektu do uloziste
			if (InsertOrUpdate(zakaznik))
			{
				//Vlozeni objektu do seznamu
				SeznamZakazniku.Add(zakaznik);
			}
		}

		/// <summary>
		/// Aktualizuje zákazníka v úložišti
		/// </summary>
		/// <param name="zakaznik">Objekt zákazník, který chceme aktualizovat v uložišti</param>
		public void UpdateZakaznik(Zakaznik zakaznik)
		{
			//Aktualizace v ulozisti
			if (InsertOrUpdate(zakaznik))
			{
				//Aktualizace v seznamu
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
		/// Smazání zákazníka z uložiště i ze seznamu zákazníků
		/// </summary>
		/// <param name="zakaznik"></param>
		public void DeleteZakaznik(Zakaznik zakaznik)
		{
			//Smazani z uloziste
			if (Delete(zakaznik))
			{
				//Smazani ze seznamu
				SeznamZakazniku.Remove(SeznamZakazniku.Find(x => x.Id == zakaznik.Id));
			}
		}
	}
}
