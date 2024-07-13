using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PcPartManagementSystems.Pages.PCPMS.Data.Manufature
{
    public class EditModel : PageModel
    {
        [BindProperty(SupportsGet = true)] public Guid? Id { get; set; }
        [BindProperty] public bl.model.Manufacturies.ManufacturiesWithCategories ret { get; set; }

        public async Task<IActionResult> OnGet()
        {
            if (Id == null)
            {
                return Page();
            }

            ret = await bl.model.Manufacturies.GetIDAsync(Id);

            return Page();
        }
    }
}
