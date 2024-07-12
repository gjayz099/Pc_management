using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PcPartManagementSystems.Pages.PCPMS.Data.Parts
{
    public class IndexModel : PageModel
    {
        [BindProperty] public List<bl.model.Parts.PartManufacture> ret {  get; set; }
        public IActionResult OnGet()
        {
            bl.sys.Acceslog("Access", "User-Gjayz", "Accses-" + bl.menu.mnu.Menu_Name_Parts);
            return Page();
        }

        public async Task<IActionResult> OnGetDisplayParts()
        {
            ret = await bl.model.Parts.GetAllAsync();

            return new JsonResult(ret);
        }

    }
}
