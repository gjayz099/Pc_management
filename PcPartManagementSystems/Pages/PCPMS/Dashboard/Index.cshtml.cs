using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PcPartManagementSystems.Pages.PCPMS.Dashboard
{
    public class IndexModel : PageModel
    {

        [BindProperty] public decimal TotalPrice { get; set; }
        [BindProperty] public int CountCus { get; set; }
        [BindProperty] public int CountManu { get; set; }
        [BindProperty] public List<bl.model.Sales.SaleWithCustomer> ret { get; set; }

        public IActionResult OnGet()
        {
      
            var _ps = new _session();
            if (!_ps.IsUserLoggedIn(HttpContext)) { return RedirectToPage("/Index"); }
            bl.sys.Acceslog("Access", "User-Gjayz", "Accses-" + bl.menu.mnu.Menu_Name_Dashboard);
            return Page();
        }



        public async Task<IActionResult> OnGetCountCus()
        {
            CountCus = await bl.dto.Customer.CountCustomerAsync();

            return new JsonResult(CountCus);
        }

        public async Task<IActionResult> OnGetCountManu()
        {
            CountManu = await bl.dto.Manufacturies.CountManuAsync();

            return new JsonResult(CountManu);
        }


        public async Task<IActionResult> OnGetSumSale()
        {
            TotalPrice = await bl.dto.Sales.GetSumSaleAsync();

            return new JsonResult(TotalPrice);
        }

        public async Task<IActionResult> OnGetDisplayData()
        {
            ret = await bl.model.Sales.GetAllAsync();

            return new JsonResult(ret);
        }

    }
}
