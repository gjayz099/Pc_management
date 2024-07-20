using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace PcPartManagementSystems.Pages.PCPMS.Buy
{

    public class BuyModel : PageModel
    {
        [BindProperty] public bl.dto.Customer dt { get; set; }
        [BindProperty] public bl.model.Customer ret { get; set; }
        [BindProperty] public bl.dto.TotalPriceBuy dtTP { get; set; }
        [BindProperty] public List<bl.dto.Sales> dts { get; set; }
        [BindProperty] public string error { get; set; }
        public IActionResult OnGet()
        {
            var _ps = new _session();
            if (!_ps.IsUserLoggedIn(HttpContext)) { return RedirectToPage("/Index"); }
            bl.sys.Acceslog("Access", "User-Gjayz", "Accses-" + bl.menu.mnu.Menu_Name_Buy);
            return Page();
        }


        public async Task<IActionResult> OnPostInsertBuyCustomer()
        {
    
            error =  await bl.dto.Customer.SavedataAll(dt, dts, dtTP);
            if (!string.IsNullOrEmpty(error))
            {
                TempData[bl.refs.ErrorMessage] = error;
                return RedirectToPage();
            }

            if(dts == null || dts.Count == 0)
            {
                TempData[bl.refs.ErrorMessage] = "Buy Item Is Null";
                return RedirectToPage();
            }
            TempData[bl.refs.SeccessMessage] = $@"Succes to Buy Item!";

            return RedirectToPage("/PCPMS/buy/Index");


        }
    }
}
