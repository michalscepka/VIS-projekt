using System;
using System.Collections;
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
    public class RezervaceFiltrModel : PageModel
    {
        public IEnumerable<Rezervace> RezervaceList { get; set; }
        public Zakaznik PrihlasenyZakaznik { get; set; }

        public RezervaceFiltrModel()
		{
            PrihlasenyZakaznik = UzivateleHelper.Instance.GetPrihlasenyZakaznik();
            RezervaceList = RezervaceHelper.Instance.GetRezervace(PrihlasenyZakaznik.Id);
		}

        public void OnGet()
        {
        }

        /// <summary>
		/// Přesměruje na stránku RezervaceDetail
		/// </summary>
        /// <param name="id">ID rezervace</param>
        /// <returns>Stránka RezervaceDetail</returns>
        public RedirectToPageResult OnPostDetail(int id)
        {
            return new RedirectToPageResult("RezervaceDetail", new { id });
        }
    }
}
