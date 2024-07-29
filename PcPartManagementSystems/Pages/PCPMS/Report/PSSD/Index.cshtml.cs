using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PcPartManagementSystems.Pages.PCPMS.Report.PSSD
{
    public class IndexModel : PageModel
    {

        [BindProperty] public DateTime startDate { get; set; } = DateTime.Now.Date;
        [BindProperty] public DateTime endDate { get; set; } = DateTime.Now.Date;
        [BindProperty] public List<bl.model.Report.ReportDataUserCat> rt { get; set; }
        [BindProperty] public byte[] ReportFile { get; set; }
        [BindProperty] public bl.model.Users usr { get; set; }
        [BindProperty] public string error { get; set; }
        public IActionResult OnGet()
        {
            var _ps = new _session();
            if (!_ps.IsUserLoggedIn(HttpContext)) { return RedirectToPage("/Index"); }
            bl.sys.Acceslog("Access", "User-Gjayz", "Accses-" + bl.menu.mnu.Menu_Name_P_S_S_D);
            return Page();

        }


        public async Task<IActionResult> OnPostCreateAddReport()
        {
            var _ps = new _session();
            var user = _ps.GetSessionValue(HttpContext, "_UserName");
            usr = await bl.model.Users.CheackUserHave(user);

            error = await bl.report.PSSD.CreateGenerateReport(startDate, endDate, usr.Id);

            if (!string.IsNullOrEmpty(error))
            {
                TempData[bl.refs.ErrorMessage] = error;
                return RedirectToPage();
            }

            return RedirectToPage();
        }


        public async Task<IActionResult> OnGetSuccessDisplayData()
        {
            rt = await bl.model.Report.GetSuccessPSCAllAsync(bl.refs.PSSD);

            return new JsonResult(rt);
        }
        public async Task<IActionResult> OnGetPindingDisplayData()
        {
            rt = await bl.model.Report.GetPendingPSCAllAsync(bl.refs.PSSD);

            return new JsonResult(rt);
        }

        public async Task<IActionResult> OnGetDownloadXLS(Guid id)
        {
            byte[] reportFile = await bl.report.PSSD.CraeteToDownlodReport(id);

            return File(reportFile, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", bl.refs.PSSD + ".xlsx");
        }

    }
}
