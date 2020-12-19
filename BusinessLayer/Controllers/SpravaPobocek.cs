﻿using BusinessLayer.BO;
using DataLayer.TableDataGateways;
using DTO.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLayer.Controllers
{
	public class SpravaPobocek
	{
        private static readonly object m_LockObj = new object();
        private static SpravaPobocek m_Instance;

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
        /// Seznam vsech poboček
        /// </summary>
        public List<Pobocka> Pobocky { get; }

        /// <summary>
        /// Celkový počet zaměstnanců
        /// </summary>
        public int CelkovyPocetPobocek => Pobocky.Count;


        /// <summary>
        /// Objekt spravy zamestnancu
        /// </summary>
        private SpravaPobocek()
        {
            Pobocky = new List<Pobocka>();
            NacteniZUloziste();
        }

        private bool NacteniZUloziste()
        {
			if (PobockaGW.Instance.Load(out List<PobockaDTO> lstPobocky, out string errMsg))
			{
				if (lstPobocky != null)
				{
					foreach (var item in lstPobocky)
					{
						Pobocky.Add(new Pobocka()
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
				//Zalogujeme někam chybu
				throw new Exception($"Pobocky: NacteniZUloziste \n{errMsg}");
			}
		}

        public bool SaveAll()
        {
            string errMsg = string.Empty;

            //Pomocny seznam pro ukladani
            List<PobockaDTO> lstPobocky = new List<PobockaDTO>();

            var maxID = Pobocky.Max(r => r.Id) + 1;

            //Naplneni zamestnancu
            foreach (var item in Pobocky)
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
                //Zalogujeme někam chybu
                throw new Exception($"Pobocky: ZapisDoUloziste \n{errMsg}");
            }
            return true;
        }

        /// <summary>
        /// Vložení nového zaměstnance
        /// </summary>
        /// <param name="pobocka"> Objek třídy zaměstnanec</param>
        public void AddPobocka(Pobocka pobocka)
        {
            Pobocky.Add(pobocka);
        }

        /// <summary>
		/// Vyhledani zaměstnance podle jeho ID
		/// </summary>
		/// <param name="id">ID zaměstnance</param>
		/// <returns>Vrací instanci objektu zaměstnanec nebo null pokud se nic nenašlo</returns>
		public Pobocka FindPobocka(int id)
        {
            //Zaporne ID značí neuložený nový záznam, takže ho asi nevyhledáme podle iD
            if (id < 0)
                return null;

            //Ověříme, že nemáme knihu již v načteném seznamu, pokud ano tak tento objekt vrátíme
            Pobocka pobocka = Pobocky.Find(x => x.Id == id);
            if (pobocka != null)
                return pobocka;
            else
                return null;

            //Nebyl nalezen objekt v seznamu, tak zkusíme uložíště

            /*if (VozidloGW.Instance.Find(id, out VozidloDTO vozidloDTO, out string errMsg))
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
            }*/
        }
    }
}
