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
    public class RezervaceDetailModel : PageModel
    {
        public Rezervace SelectedRezervace { get; set; }
        public int Status { get; set; }
        public IEnumerable<Vozidlo> Vozidla { get; set; }

        [BindProperty]
        public DateTime DatumProdlouzeni { get; set; }

        public RezervaceDetailModel()
		{
            Status = 0;
        }

        public void OnGet(int id)
        {
            SelectedRezervace = SpravaRezervaci.Instance.FindRezervace(id);
        }

        /// <summary>
		/// Pokusí se prodloužit rezervaci vozidla
		/// </summary>
        /// <param name="id">ID rezervace</param>
        public void OnPostProdlouzit(int id)
        {
            SelectedRezervace = SpravaRezervaci.Instance.FindRezervace(id);

            if (DatumProdlouzeni == DateTime.MinValue)
            {
                Status = 3;
                return;
            }
            if (DatumProdlouzeni < SelectedRezervace.DatumKonceRezervace)
            {
                Status = 4;
                return;
            }

            DateTime originalKonecDate = SelectedRezervace.DatumKonceRezervace;
            SelectedRezervace.DatumKonceRezervace = DatumProdlouzeni;

			if (RezervaceHelper.Instance.CanExtendReservation(SelectedRezervace))
            {
                SelectedRezervace.Cena = ((SelectedRezervace.DatumKonceRezervace - SelectedRezervace.DatumZacatkuRezervace).Days + 1) * SelectedRezervace.Vozidlo.CenaZaDen;
                SelectedRezervace.Kauce = SelectedRezervace.Cena > 1000 ? Convert.ToInt32(SelectedRezervace.Cena * 0.1) : 0;
                SpravaRezervaci.Instance.UpdateRezervace(SelectedRezervace);
                EmailHelper.Instance.SendEmail();
                Status = 1;
            }
            else
            {
                SelectedRezervace.DatumKonceRezervace = originalKonecDate;
                Vozidla = SpravaVozidel.Instance.SeznamVozidel;
                Vozidla = Vozidla.Where(x => VozidlaHelper.Instance.IsVehicleFree(x, SelectedRezervace.DatumZacatkuRezervace, DatumProdlouzeni));
                Vozidla = Vozidla.Where(x => x.Aktivni);
                Status = 2;
            }
        }

        /// <summary>
		/// Přesměruje na stránku VozidloDetail
		/// </summary>
        /// <param name="id">ID vozidla</param>
        /// <returns>Stránka VozidloDetail</returns>
        public RedirectToPageResult OnPostDetail(int id)
        {
            return new RedirectToPageResult("VozidloDetail", new { id });
        }
    }
}
