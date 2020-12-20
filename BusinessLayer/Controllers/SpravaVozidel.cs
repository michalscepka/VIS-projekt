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
	/// Třída zodpovědná za správu vozidel
	/// </summary>
	public class SpravaVozidel
	{
		/// <summary>
		/// Privátní statická proměnná udržující vytvořenou instanci třídy
		/// </summary>
		private static SpravaVozidel m_Instance;

		/// <summary>
		/// Pomocný objekt sloužící pro zajištění thread safe přístupu při vytváření instance
		/// </summary>
		private static readonly object m_LockObj = new object();

		/// <summary>
		/// Seznam všech zamestnancu v systému
		/// </summary>
		public List<Vozidlo> SeznamVozidel { get; }

		/// <summary>
		/// Statická vlastnost třídy, přes kterou se přistupuje ke třídě jako singletonu
		/// </summary>
		public static SpravaVozidel Instance
		{
			get
			{
				lock (m_LockObj)
				{
					return m_Instance ??= new SpravaVozidel();
				}
			}
		}

		/// <summary>
		/// Privátní konstruktor, třídu nelze vytvořit jinak, než přes přístup na vlastnost Instance
		/// V rámci konstruktoru načte všechny vozidla z uložiště
		/// </summary>
		private SpravaVozidel()
		{
			SeznamVozidel = new List<Vozidlo>();

			if (VozidloGW.Instance.LoadAll(out List<VozidloDTO> list, out string errMsg))
			{
				if (list != null)
				{
					foreach (VozidloDTO item in list)
					{
						SeznamVozidel.Add(new Vozidlo()
						{
							Id = item.Id,
							Znacka = item.Znacka,
							Model = item.Model,
							SPZ = item.SPZ,
							CenaZaDen = item.CenaZaDen,
							PocetDveri = item.PocetDveri,
							Motor = item.Motor,
							Spotreba = item.Spotreba,
							Obrazek = item.Obrazek,
							Aktivni = item.Aktivni,
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
		/// Vložení nebo aktualizace objektu vozidlo v uložišti
		/// </summary>
		/// <param name="vozidlo"></param>
		/// <returns>True, pokud se insert/update povedl</returns>
		private bool InsertOrUpdate(Vozidlo vozidlo)
		{
			VozidloDTO vozidloDTO = new VozidloDTO()
			{
				Id = vozidlo.Id,
				Znacka = vozidlo.Znacka,
				Model = vozidlo.Model,
				SPZ = vozidlo.SPZ,
				CenaZaDen = vozidlo.CenaZaDen,
				PocetDveri = vozidlo.PocetDveri,
				Motor = vozidlo.Motor,
				Spotreba = vozidlo.Spotreba,
				Obrazek = vozidlo.Obrazek,
				Aktivni = vozidlo.Aktivni,
				PobockaId = vozidlo.Pobocka.Id
			};

			if (VozidloGW.Instance.InsertOrUpdate(vozidloDTO, out string errMsg))
			{
				return true;
			}
			else
			{
				throw new DataException($"Nastala chyba při vložení/aktualizaci zamestnance v uložišti\n {errMsg}");
			}
		}

		/// <summary>
		/// Smazání vozidla z úložiště
		/// </summary>
		/// <param name="vozidlo">Objekt, který chceme smazat</param>
		/// <returns>True, pokud se povedlo smazání</returns>
		private bool Delete(Vozidlo vozidlo)
		{
			if (VozidloGW.Instance.Delete(vozidlo.Id, out string errMsg))
			{
				return true;
			}
			else
			{
				throw new DataException($"Nastala chyba při mazání knihy z uložiště\n {errMsg}");
			}
		}

		/// <summary>
		/// Uložení všech vozidel
		/// </summary>
		public void SaveAllData()
		{
			List<VozidloDTO> zamestnanciDTO = new List<VozidloDTO>();
			foreach (Vozidlo item in SeznamVozidel)
			{
				zamestnanciDTO.Add(new VozidloDTO()
				{
					Id = item.Id,
					Znacka = item.Znacka,
					Model = item.Model,
					SPZ = item.SPZ,
					CenaZaDen = item.CenaZaDen,
					PocetDveri = item.PocetDveri,
					Motor = item.Motor,
					Spotreba = item.Spotreba,
					Obrazek = item.Obrazek,
					Aktivni = item.Aktivni,
					PobockaId = item.Pobocka.Id
				});
			}

			if (!VozidloGW.Instance.SaveAll(zamestnanciDTO, out string errMsg))
			{
				throw new DataException($"Nastala chyba při zápisu knih do uložiště\n {errMsg}");
			}
		}

		/// <summary>
		/// Vyhledání vozidla podle jeho ID
		/// </summary>
		/// <param name="id">ID vozidla</param>
		/// <returns>Vrací instanci objektu vozidlo nebo null pokud se nic nenašlo</returns>
		public Vozidlo FindVozidlo(int id)
		{
			//Zaporne ID značí neuložený nový záznam
			if (id < 0)
				return null;

			//Ověříme, že nemáme objekt již v načteném seznamu, pokud ano tak tento objekt vrátíme
			Vozidlo vozidlo = SeznamVozidel.Find(x => x.Id == id);
			if (vozidlo != null)
				return vozidlo;

			//Nebyl nalezen objekt v seznamu, tak zkusíme uložíště
			if (VozidloGW.Instance.Find(id, out VozidloDTO vozidloDTO, out string errMsg))
			{
				return new Vozidlo()
				{
					Id = vozidloDTO.Id,
					Znacka = vozidloDTO.Znacka,
					Model = vozidloDTO.Model,
					SPZ = vozidloDTO.SPZ,
					CenaZaDen = vozidloDTO.CenaZaDen,
					PocetDveri = vozidloDTO.PocetDveri,
					Motor = vozidloDTO.Motor,
					Spotreba = vozidloDTO.Spotreba,
					Obrazek = vozidloDTO.Obrazek,
					Aktivni = vozidloDTO.Aktivni,
					Pobocka = new Pobocka() { Id = vozidloDTO.PobockaId }
				};
			}
			else
			{
				throw new DataException($"Nastala chyba při vyhledání zaměstnance v uložišti\n {errMsg}");
			}
		}

		/// <summary>
		/// Vloží nové vozidlo do seznamu vozidel a současně i do DB
		/// </summary>
		/// <param name="vozidlo">Objekt vozidlo, ktrerý budeme vkládat</param>
		public void AddVozidlo(Vozidlo vozidlo)
		{
			//Vlozeni objektu do uloziste
			if (InsertOrUpdate(vozidlo))
			{
				//Vlozeni objektu do seznamu
				SeznamVozidel.Add(vozidlo);
			}
		}

		/// <summary>
		/// Aktualizuje vozidlo v úložišti
		/// </summary>
		/// <param name="vozidlo">Objekt vozidlo, který chceme aktualizovat v uložišti</param>
		public void UpdateVozidlo(Vozidlo vozidlo)
		{
			//Aktualizace v ulozisti
			if (InsertOrUpdate(vozidlo))
			{
				//Aktualizace v seznamu
				Vozidlo updatedVozidlo = SeznamVozidel.Find(x => x.Id == vozidlo.Id);
				updatedVozidlo.Id = vozidlo.Id;
				updatedVozidlo.Znacka = vozidlo.Znacka;
				updatedVozidlo.Model = vozidlo.Model;
				updatedVozidlo.SPZ = vozidlo.SPZ;
				updatedVozidlo.CenaZaDen = vozidlo.CenaZaDen;
				updatedVozidlo.PocetDveri = vozidlo.PocetDveri;
				updatedVozidlo.Motor = vozidlo.Motor;
				updatedVozidlo.Spotreba = vozidlo.Spotreba;
				updatedVozidlo.Obrazek = vozidlo.Obrazek;
				updatedVozidlo.Aktivni = vozidlo.Aktivni;
				updatedVozidlo.Pobocka.Id = vozidlo.Pobocka.Id;
			}
		}

		/// <summary>
		/// Smazání vozidla z úložiště i ze seznamu zaměstnanců
		/// </summary>
		/// <param name="vozidlo"></param>
		public void DeleteVozidlo(Vozidlo vozidlo)
		{
			//Smazani z uloziste
			if (Delete(vozidlo))
			{
				//Smazani ze seznamu
				SeznamVozidel.Remove(SeznamVozidel.Find(x => x.Id == vozidlo.Id));
			}
		}
	}
}
