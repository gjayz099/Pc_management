using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PcPartManagementSystems.Pages.PCPMS.Data.Categories
{
    public class IndexModel : PageModel
    {

        [BindProperty] public List<bl.model.Categories> ret {  get; set; }

        [BindProperty] public bl.dto.Categories dto { get; set; }
        public IActionResult OnGet()
        {
            var _ps = new _session();
            if (!_ps.IsUserLoggedIn(HttpContext)) { return RedirectToPage("/Index"); }
            bl.sys.Acceslog("Access", "User-Gjayz", "Accses-" + bl.menu.mnu.Menu_Name_Category);
            return Page();
        }


        public async Task<IActionResult> OnGetDisplayData()
        {

            // Call the asynchronous method to fetch data
            ret = await bl.model.Categories.GetAllAsync();

            // Return the fetched data as a JSON result
            return new JsonResult(ret);
        }

        public async Task<IActionResult> OnPostInsertCategory()
        {

            var error = await bl.dto.Categories.InsertAllAsync(dto);

            if (!string.IsNullOrEmpty(error))
            {
                TempData[bl.refs.ErrorMessage] = error;
                return RedirectToPage();
            }
            // Set success message
            TempData[bl.refs.SeccessMessage] = $@"{dto.CategoryName} successfully inserted!";

    

            return RedirectToPage();
      
        }
    }
}
