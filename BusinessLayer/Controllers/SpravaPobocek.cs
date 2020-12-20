using BusinessLayer.BO;
using DataLayer.TableDataGateways;
using DTO.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLayer.Controllers
{
    /// <summary>
    /// Třída zodpovědná za správu poboček
    /// </summary>
	public class SpravaPobocek
	{
        /// <summary>
		/// Privátní statická proměnná udržující vytvořenou instanci třídy
		/// </summary>
        private static SpravaPobocek m_Instance;

        /// <summary>
        /// Pomocný objekt sloužící pro zajištění thread safe přístupu při vytváření instance
        /// </summary>
        private static readonly object m_LockObj = new object();

        /// <summary>
        /// Seznam všech poboček
        /// </summary>
        public List<Pobocka> SeznamPobocek { get; }

        /// <summary>
        /// Statická vlastnost třídy, přes kterou se přistupuje ke třídě jako singletonu
        /// </summary>
        public static SpravaPobocek Instance
        {
            get
            {
                lock (m_LockObj)
                {
                    return m_Instance ??= new SpravaPobocek();
                }
            }
        }

        /// <summary>
		/// Privátní konstruktor, třídu nelze vytvořit jinak, než přes přístup na vlastnost Instance
		/// V rámci konstruktoru načte všechny pobočky z uložiště
		/// </summary>
        private SpravaPobocek()
        {
            SeznamPobocek = new List<Pobocka>();
            NacteniZUloziste();
        }

        /// <summary>
        /// Načtení úložiště
        /// </summary>
        /// <returns>True načtení se provedlo</returns>
        private bool NacteniZUloziste()
        {
			if (PobockaGW.Instance.Load(out List<PobockaDTO> lstPobocky, out string errMsg))
			{
				if (lstPobocky != null)
				{
					foreach (var item in lstPobocky)
					{
						SeznamPobocek.Add(new Pobocka()
						{
							Id = item.Id,
							Mesto = item.Mesto,
							Ulice = item.Ulice,
							Telefon = item.Telefon
						});
					}
				}
				return true;
			}
			else
			{
				throw new Exception($"Pobocky: NacteniZUloziste \n{errMsg}");
			}
		}

        /// <summary>
        /// Uložení dat do úložiště
        /// </summary>
        /// <returns>True načtení se provedlo, False nastala chyba</returns>
        public bool SaveAll()
        {
            string errMsg = string.Empty;
            List<PobockaDTO> lstPobocky = new List<PobockaDTO>();
            var maxID = SeznamPobocek.Max(r => r.Id) + 1;

            foreach (var item in SeznamPobocek)
            {
                lstPobocky.Add(
                    new PobockaDTO()
                    {
                        Id = item.Id == -1 ? maxID++ : item.Id,
                        Mesto = item.Mesto,
                        Ulice = item.Ulice,
                        Telefon = item.Telefon
                    });
            }

            if (!PobockaGW.Instance.Save(lstPobocky, out errMsg))
            {
                throw new Exception($"Pobocky: SaveAll \n{errMsg}");
            }
            return true;
        }

        /// <summary>
        /// Vložení nové pobočky
        /// </summary>
        /// <param name="pobocka"> Objek třídy pobočka</param>
        public void AddPobocka(Pobocka pobocka)
        {
            //Vlozeni objektu do seznamu
            SeznamPobocek.Add(pobocka);
        }

        /// <summary>
		/// Vyhledani pobočky podle jejího ID
		/// </summary>
		/// <param name="id">ID pobočky</param>
		/// <returns>Vrací instanci objektu pobočka nebo null pokud se nic nenašlo</returns>
		public Pobocka FindPobocka(int id)
        {
            //Zaporne ID značí neuložený nový záznam
            if (id < 0)
                return null;

            //Ověříme, že nemáme objekt již v načteném seznamu, pokud ano tak tento objekt vrátíme
            Pobocka pobocka = SeznamPobocek.Find(x => x.Id == id);
            return pobocka ?? null;
        }
    }
}
