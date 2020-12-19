using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer.BO;
using BusinessLayer.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PresentationLayer;

namespace WebApp.Pages
{
    public class VozidloDetailModel : PageModel
    {
        public Vozidlo Vozidlo { get; set; }
        public string Message { get; set; }
        public int Status { get; set; }
        public Rezervace NewRezervace { get; set; }

        public VozidloDetailModel()
		{
            Status = 0;
        }

        public void OnGet(int id)
        {
            Vozidlo = VozidlaHelper.Instance.GetVozidlo(id);
        }

        [BindProperty]
        public DateTime DatumStart { get; set; }
        [BindProperty]
        public DateTime DatumKonec { get; set; }
        public void OnPostRezervovat(int id)
        {
            Vozidlo = SpravaVozidel.Instance.FindVozidlo(id);

            if (DatumStart == DateTime.MinValue || DatumKonec == DateTime.MinValue)
			{
                Status = 3;
                return;
			}
            if (DatumStart > DatumKonec)
			{
                Status = 4;
                return;
			}

            //TODO vyresit automaticke pridavani id = -1
            NewRezervace = new Rezervace()
            {
                Id = -1,
                DatumZacatkuRezervace = DatumStart,
                DatumKonceRezervace = DatumKonec,
                Vozidlo = Vozidlo,
                Zakaznik = UzivateleHelper.Instance.GetPrihlasenyZakaznik()
            };

            if (RezervaceHelper.Instance.CanCreateReservation(NewRezervace))
            {
                NewRezervace.Cena = ((NewRezervace.DatumKonceRezervace - NewRezervace.DatumZacatkuRezervace).Days + 1) * NewRezervace.Vozidlo.CenaZaDen;
                NewRezervace.Kauce = NewRezervace.Cena > 1000 ? Convert.ToInt32(NewRezervace.Cena * 0.1) : 0;
                SpravaRezervaci.Instance.AddRezervace(NewRezervace);
                EmailHelper.Instance.SendEmail();
                Status = 1;
            }
            else
            {
                Status = 2;
            }
        }
    }
}
