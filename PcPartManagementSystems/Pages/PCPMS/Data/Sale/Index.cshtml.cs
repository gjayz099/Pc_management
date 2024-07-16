using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PcPartManagementSystems.Pages.PCPMS.Data.Sale
{
    public class IndexModel : PageModel
    {

        [BindProperty] public List<bl.model.Sales.SaleWithCustomer> ret { get; set; }

        public IActionResult OnGet()
        {
            var _ps = new _session();
            if (!_ps.IsUserLoggedIn(HttpContext)) { return RedirectToPage("/Index"); }

            bl.sys.Acceslog("Access", "User-Gjayz", "Accses-" + bl.menu.mnu.Menu_Name_Sale);
            return Page();
       
        }

        public async Task<IActionResult> OnGetDisplayData()
        {
            ret = await bl.model.Sales.GetAllAsync();

            return new JsonResult(ret);
        }
    }
}
