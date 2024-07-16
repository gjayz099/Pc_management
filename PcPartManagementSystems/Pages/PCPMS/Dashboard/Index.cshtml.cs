using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PcPartManagementSystems.Pages.PCPMS.Dashboard
{
    public class IndexModel : PageModel
    {
        public IActionResult OnGet()
        {
            var _ps = new _session();
            if (!_ps.IsUserLoggedIn(HttpContext)) { return RedirectToPage("/Index"); }
            bl.sys.Acceslog("Access", "User-Gjayz", "Accses-" + bl.menu.mnu.Menu_Name_Dashboard);
            return Page();
        }
    }
}
