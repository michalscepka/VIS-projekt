using BusinessLayer.BO;
using DataLayer.TableDataGateways;
using DTO.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace BusinessLayer.Controllers
{
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
		/// Celkový počet včech zamestnancu v systému
		/// </summary>
		public int CelkovyPocetVozidel => SeznamVozidel.Count;

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
		/// V rámci konstruktoru načte všechny pobocky z uložiště
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
		/// Smazání zaměstnance z Uložiště
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
		/// Uložení všech zaměstnanců
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
		/// Vyhledani zaměstnance podle jeho ID
		/// </summary>
		/// <param name="id">ID zaměstnance</param>
		/// <returns>Vrací instanci objektu zaměstnanec nebo null pokud se nic nenašlo</returns>
		public Vozidlo FindVozidlo(int id)
		{
			//Zaporne ID značí neuložený nový záznam, takže ho asi nevyhledáme podle iD
			if (id < 0)
				return null;

			//Ověříme, že nemáme knihu již v načteném seznamu, pokud ano tak tento objekt vrátíme
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
		/// Vloží noveho zaměstnance do seznamu zaměstnanců a současně i do DB
		/// </summary>
		/// <param name="vozidlo">Objekt zaměstnanec, ktrerý budeme vkládat</param>
		public void AddVozidlo(Vozidlo vozidlo)
		{
			if (InsertOrUpdate(vozidlo))
			{
				SeznamVozidel.Add(vozidlo);
			}
		}

		/// <summary>
		/// Aktualizuje zaměstnance v uložišti
		/// </summary>
		/// <param name="vozidlo">Objekt zaměstnanec, který chceme aktualizovat v uložišti</param>
		public void UpdateVozidlo(Vozidlo vozidlo)
		{
			//Aktualizace v ulozisti
			if (InsertOrUpdate(vozidlo))
			{
				//Aktualizovat musime i objekt v seznamu
				Vozidlo updatedVozidlo = SeznamVozidel.Find(x => x.Id == vozidlo.Id);
				updatedVozidlo.Id = vozidlo.Id;
				updatedVozidlo.Znacka = vozidlo.Znacka;
				updatedVozidlo.Model = vozidlo.Model;
				updatedVozidlo.SPZ = vozidlo.SPZ;
				updatedVozidlo.CenaZaDen = vozidlo.CenaZaDen;
				updatedVozidlo.PocetDveri = vozidlo.PocetDveri;
				updatedVozidlo.Motor = vozidlo.Motor;
				updatedVozidlo.Spotreba = vozidlo.Spotreba;
				updatedVozidlo.Aktivni = vozidlo.Aktivni;
				updatedVozidlo.Pobocka.Id = vozidlo.Pobocka.Id;
			}
		}

		/// <summary>
		/// Smazání zaměstnance z uložiště i ze seznamu zaměstnanců
		/// </summary>
		/// <param name="vozidlo"></param>
		public void DeleteVozidlo(Vozidlo vozidlo)
		{
			//Mažeme objekt kniha v uložišti
			if (Delete(vozidlo))
			{
				//Musíme smazat i v seznamu knih
				//SeznamVozidel.Remove(vozidlo);
				SeznamVozidel.Remove(SeznamVozidel.Find(x => x.Id == vozidlo.Id));
			}
		}
	}
}
