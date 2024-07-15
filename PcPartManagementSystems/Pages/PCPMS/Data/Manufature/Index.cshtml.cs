using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PcPartManagementSystems.Pages.PCPMS.Data.Manufature
{
    public class IndexModel : PageModel
    {

        [BindProperty] List<bl.model.Manufacturies.ManufacturiesWithCategories> ret {  get; set; }
        public IActionResult OnGet()
        {

            bl.sys.Acceslog("Access", "User-Gjayz", "Accses-" + bl.menu.mnu.Menu_Name_Manufature);
            return Page();

        }
        public async Task<IActionResult> OnGetDisplayData()
        {
            ret = await bl.model.Manufacturies.GetAllAsync();

            return new JsonResult(ret);
        }
 
    }
}
