using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PcPartManagementSystems.Pages.PCPMS.Buy
{

    public class ViewModel : PageModel
    {
        [BindProperty(SupportsGet = true)] public Guid Id { get; set; }
        [BindProperty] public List<bl.model.Customer.CusToTotalBuyItme> cts {  get; set; }

        [BindProperty] public bl.model.Customer.CusToTotalBuy ctr { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var _ps = new _session();
            if (!_ps.IsUserLoggedIn(HttpContext)) { return RedirectToPage("/Index"); }
            ctr = await bl.model.Customer.GetItemToBuyCustomerAsync(Id);
            return Page();
        }

        public async Task<IActionResult> OnGetDataDisplay()
        {
            cts = await bl.model.Customer.GetItemToBuyAsync(Id);


            return new JsonResult(cts);
        }


    }
}
