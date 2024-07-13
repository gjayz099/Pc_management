using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PcPartManagementSystems.Pages.PCPMS.Data.Manufature
{
    public class IndexModel : PageModel
    {

        [BindProperty] List<bl.model.Manufacturies.ManufacturiesWithCategories> ret {  get; set; }
        public IActionResult OnGet()
        {
            return Page();
        }
        public async Task<IActionResult> OnGetDisplayData()
        {
            ret = await bl.model.Manufacturies.GetAllAsync();

            return new JsonResult(ret);
        }
 
    }
}
