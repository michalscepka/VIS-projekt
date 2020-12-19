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
    public class VozidlaFiltrModel : PageModel
    {
        public IEnumerable<Vozidlo> Vozidla { get; set; }
        public string Message { get; set; }

        public VozidlaFiltrModel()
		{
            Vozidla = VozidlaHelper.Instance.GetVozidla();
            Vozidla = Vozidla.Where(x => x.Aktivni);
        }

        public void OnGet()
        {
        }

        [BindProperty]
        public string ZnackaInput { get; set; }
        [BindProperty]
        public string ModelInput { get; set; }
        [BindProperty]
        public string MestoInput { get; set; }
        public void OnPostFilter()
		{
            if (!string.IsNullOrEmpty(ZnackaInput))
                Vozidla = Vozidla.Where(x => x.Znacka.ToLower().Contains(ZnackaInput.ToLower()));
            if (!string.IsNullOrEmpty(ModelInput))
                Vozidla = Vozidla.Where(x => x.Model.ToLower().Contains(ModelInput.ToLower()));
            if (!string.IsNullOrEmpty(MestoInput))
                Vozidla = Vozidla.Where(x => x.Pobocka.Mesto.ToLower().Contains(MestoInput.ToLower()));
            if (Vozidla.Count() <= 0)
                Message = string.Format("Omlouváme se ale pro hledaný výraz '{0} {1} {2}' jsme nenašli žádné vozidlo.", ZnackaInput, ModelInput, MestoInput);
		}

        public RedirectToPageResult OnPostDetail(int id)
        {
            return new RedirectToPageResult("VozidloDetail", new { id });
        }
    }
}
