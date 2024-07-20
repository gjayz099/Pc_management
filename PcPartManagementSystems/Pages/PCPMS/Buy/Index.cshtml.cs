using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PcPartManagementSystems.Pages.PCPMS.Buy
{
    public class IndexModel : PageModel
    {
        [BindProperty] public List<bl.model.Customer.CusToTotalBuy> ret {  get; set; }
        public IActionResult OnGet()
        {
            var _ps = new _session();
            if (!_ps.IsUserLoggedIn(HttpContext)) { return RedirectToPage("/Index"); }
            bl.sys.Acceslog("Access", "User-Gjayz", "Accses-" + bl.menu.mnu.Menu_Name_Dashboard);
            return Page();
        }

        public async Task<IActionResult> OnGetDisplayData()
        {
            ret = await bl.model.Customer.GetAllAsync();

            return new JsonResult(ret);
        }
    }
}
