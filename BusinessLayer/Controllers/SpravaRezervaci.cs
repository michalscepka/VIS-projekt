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
	/// Třída zodpovědná za správu rezervací
	/// </summary>
	public class SpravaRezervaci
	{
		/// <summary>
		/// Privátní statická proměnná udržující vytvořenou instanci třídy
		/// </summary>
		private static SpravaRezervaci m_Instance;

		/// <summary>
		/// Pomocný objekt sloužící pro zajištění thread safe přístupu při vytváření instance
		/// </summary>
		private static readonly object m_LockObj = new object();

		/// <summary>
		/// Seznam všech rezervací v systému
		/// </summary>
		public List<Rezervace> SeznamRezervaci { get; }

		/// <summary>
		/// Statická vlastnost třídy, přes kterou se přistupuje ke třídě jako singletonu
		/// </summary>
		public static SpravaRezervaci Instance
		{
			get
			{
				lock (m_LockObj)
				{
					return m_Instance ??= new SpravaRezervaci();
				}
			}
		}

		/// <summary>
		/// Privátní konstruktor, třídu nelze vytvořit jinak, než přes přístup na vlastnost Instance
		/// V rámci konstruktoru načte všechny rezervace z uložiště
		/// </summary>
		private SpravaRezervaci()
		{
			SeznamRezervaci = new List<Rezervace>();

			if (RezervaceGW.Instance.LoadAll(out List<RezervaceDTO> list, out string errMsg))
			{
				if (list != null)
				{
					foreach (RezervaceDTO item in list)
					{
						SeznamRezervaci.Add(new Rezervace()
						{
							Id = item.Id,
							DatumZacatkuRezervace = item.DatumZacatkuRezervace,
							DatumKonceRezervace = item.DatumKonceRezervace,
							Cena = item.Cena,
							Kauce = item.Kauce,
							Zakaznik = new Zakaznik() { Id = item.ZakaznikId},
							Vozidlo = new Vozidlo() { Id = item.VozidloId}
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
		/// Vložení nebo aktualizace objektu rezervace v úložišti
		/// </summary>
		/// <param name="rezervace"></param>
		/// <returns>True, pokud se insert/update povedl</returns>
		private bool InsertOrUpdate(Rezervace rezervace)
		{
			RezervaceDTO rezervaceDTO = new RezervaceDTO()
			{
				Id = rezervace.Id,
				DatumZacatkuRezervace = rezervace.DatumZacatkuRezervace,
				DatumKonceRezervace = rezervace.DatumKonceRezervace,
				Cena = rezervace.Cena,
				Kauce = rezervace.Kauce,
				ZakaznikId = rezervace.Zakaznik.Id,
				VozidloId = rezervace.Vozidlo.Id
			};

			if (RezervaceGW.Instance.InsertOrUpdate(rezervaceDTO, out string errMsg))
			{
				rezervace.Id = rezervaceDTO.Id;
				return true;
			}
			else
			{
				throw new DataException($"Nastala chyba při vložení/aktualizaci rezervace v uložišti\n {errMsg}");
			}
		}

		/// <summary>
		/// Smazání zaměstnance z Uložiště
		/// </summary>
		/// <param name="rezervace">Objekt, který chceme smazat</param>
		/// <returns>True, pokud se povedlo smazání</returns>
		private bool Delete(Rezervace rezervace)
		{
			if (RezervaceGW.Instance.Delete(rezervace.Id, out string errMsg))
			{
				return true;
			}
			else
			{
				throw new DataException($"Nastala chyba při mazání knihy z uložiště\n {errMsg}");
			}
		}

		/// <summary>
		/// Uložení všech rezervací
		/// </summary>
		public void SaveAllData()
		{
			List<RezervaceDTO> zamestnanciDTO = new List<RezervaceDTO>();
			foreach (Rezervace item in SeznamRezervaci)
			{
				zamestnanciDTO.Add(new RezervaceDTO()
				{
					Id = item.Id,
					DatumZacatkuRezervace = item.DatumZacatkuRezervace,
					DatumKonceRezervace = item.DatumKonceRezervace,
					Cena = item.Cena,
					Kauce = item.Kauce,
					ZakaznikId = item.Zakaznik.Id,
					VozidloId = item.Vozidlo.Id
				});
			}

			if (!RezervaceGW.Instance.SaveAll(zamestnanciDTO, out string errMsg))
			{
				throw new DataException($"Nastala chyba při zápisu knih do uložiště\n {errMsg}");
			}
		}

		/// <summary>
		/// Vyhledání rezervace podle jejího ID
		/// </summary>
		/// <param name="id">ID rezervace</param>
		/// <returns>Vrací instanci objektu rezervace nebo null pokud se nic nenašlo</returns>
		public Rezervace FindRezervace(int id)
		{
			//Zaporne ID značí neuložený nový záznam
			if (id < 0)
				return null;

			//Ověříme, že nemáme objekt již v načteném seznamu, pokud ano tak tento objekt vrátíme
			Rezervace rezervace = SeznamRezervaci.Find(x => x.Id == id);
			if (rezervace != null)
				return rezervace;

			//Nebyl nalezen objekt v seznamu, tak zkusíme uložíště
			if (RezervaceGW.Instance.Find(id, out RezervaceDTO rezervaceDTO, out string errMsg))
			{
				return new Rezervace()
				{
					Id = rezervaceDTO.Id,
					DatumZacatkuRezervace = rezervaceDTO.DatumZacatkuRezervace,
					DatumKonceRezervace = rezervaceDTO.DatumKonceRezervace,
					Cena = rezervaceDTO.Cena,
					Kauce = rezervaceDTO.Kauce,
					Zakaznik = new Zakaznik() { Id = rezervaceDTO.ZakaznikId },
					Vozidlo = new Vozidlo() { Id = rezervaceDTO.VozidloId }
				};
			}
			else
			{
				throw new DataException($"Nastala chyba při vyhledání zaměstnance v uložišti\n {errMsg}");
			}
		}

		/// <summary>
		/// Vloží novou rezervaci do seznamu rezervací a současně i do DB
		/// </summary>
		/// <param name="rezervace">Objekt zaměstnanec, ktrerý budeme vkládat</param>
		public void AddRezervace(Rezervace rezervace)
		{
			//Vlozeni objektu do uloziste
			if (InsertOrUpdate(rezervace))
			{
				//Vlozeni objektu do seznamu
				SeznamRezervaci.Add(rezervace);
			}
		}

		/// <summary>
		/// Aktualizuje rezervaci v úložišti i v seznamu rezervací
		/// </summary>
		/// <param name="rezervace">Objekt zaměstnanec, který chceme aktualizovat v uložišti</param>
		public void UpdateRezervace(Rezervace rezervace)
		{
			//Aktualizace v ulozisti
			if (InsertOrUpdate(rezervace))
			{
				//Aktualizace v seznamu
				Rezervace updatedRezervace = SeznamRezervaci.Find(x => x.Id == rezervace.Id);
				updatedRezervace.Id = rezervace.Id;
				updatedRezervace.DatumZacatkuRezervace = rezervace.DatumZacatkuRezervace;
				updatedRezervace.DatumKonceRezervace = rezervace.DatumKonceRezervace;
				updatedRezervace.Cena = rezervace.Cena;
				updatedRezervace.Kauce = rezervace.Kauce;
				updatedRezervace.Zakaznik.Id = rezervace.Zakaznik.Id;
				updatedRezervace.Vozidlo.Id = rezervace.Vozidlo.Id;
			}
		}

		/// <summary>
		/// Smazání rezervace z úložiště i ze seznamu zaměstnanců
		/// </summary>
		/// <param name="rezervace"></param>
		public void DeleteRezervace(Rezervace rezervace)
		{
			//Smazani z uloziste
			if (Delete(rezervace))
			{
				//Smazani ze seznamu
				SeznamRezervaci.Remove(SeznamRezervaci.Find(x => x.Id == rezervace.Id));
			}
		}
	}
}
