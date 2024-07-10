using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PcPartManagementSystems.Pages.PCPMS.Data.Categories
{
    public class IndexModel : PageModel
    {

        [BindProperty] public List<bl.model.Categories> ret {  get; set; }
  
        public IActionResult OnGet()
        {
            return Page();
        }


        public async Task<IActionResult> OnGetDisplayData()
        {
            // Call the asynchronous method to fetch data
            ret = await bl.dto.Categories.GetAllAsync();

            // Return the fetched data as a JSON result
            return new JsonResult(ret);
        }
    }
}
