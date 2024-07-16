using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PcPartManagementSystems.Pages
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnGet()
        {
            var _ps = new _session();
            _ps.ClearSession(HttpContext);

            return RedirectToPage("/Index");
        }
    }
}
