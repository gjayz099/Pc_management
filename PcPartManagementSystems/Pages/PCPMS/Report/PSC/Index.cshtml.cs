using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.SqlServer.Management.Smo.Agent;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PcPartManagementSystems.Pages.PCPMS.Report.PSC
{
    public class IndexModel : PageModel
    {

        [BindProperty] public List<bl.model.Categories> cat { get; set; }
        [BindProperty] public bl.model.Users usr { get; set; }
        [BindProperty] public List<bl.model.Report.ReportDataUserCat> rt { get; set; }
        [BindProperty] public Guid Filter { get; set; }
        [BindProperty] public byte[] ReportFile { get; set; }
        [BindProperty] public string error { get; set; }
        public async Task<IActionResult> OnGet()
        {
            var _ps = new _session();
            if (!_ps.IsUserLoggedIn(HttpContext)) { return RedirectToPage("/Index"); }
            bl.sys.Acceslog("Access", "User-Gjayz", "Accses-" + bl.menu.mnu.Menu_Name_P_S_C);
            cat = await bl.model.Categories.GetAllAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostCreateAddReport()
        {
            var _ps = new _session();
            var user = _ps.GetSessionValue(HttpContext, "_UserName");
            usr = await bl.model.Users.CheackUserHave(user);

            var erro =  await bl.report.PSC.CreateGenerateReport(Filter, usr.Id);

            if (!string.IsNullOrEmpty(erro))
            {
                TempData[bl.refs.ErrorMessage] = erro;
                return RedirectToPage();
            }

            return RedirectToPage();
        }


        public async Task<IActionResult> OnGetSuccessDisplayData()
        {
            rt = await bl.model.Report.GetSuccessPSCAllAsync(bl.refs.PSC);

            return new JsonResult(rt);
        }
        public async Task<IActionResult> OnGetPindingDisplayData()
        {
            rt = await bl.model.Report.GetPendingPSCAllAsync(bl.refs.PSC);

            return new JsonResult(rt);
        }

        public async Task<IActionResult> OnGetDownloadXLS(Guid id)
        {
            byte[] reportFile = await bl.report.PSC.CraeteToDownlodReport(id);

            return File(reportFile, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", bl.refs.PSC + ".xlsx");
        }


    }
}
