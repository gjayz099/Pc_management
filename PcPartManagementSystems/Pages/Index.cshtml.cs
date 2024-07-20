using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PcPartManagementSystems.Pages
{
    public class IndexModel : PageModel
    {

        [BindProperty] public bl.model.Users usr {  get; set; }
        [BindProperty] public bl.dto.Users dto { get; set; }
        [BindProperty] public string error { get; set; }

        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            var _ps = new _session();
            if (_ps.IsUserLoggedIn(HttpContext)) { return RedirectToPage("/PCPMS/Dashboard/Index"); }
            return Page();
        }
        public async Task<IActionResult> OnPostUserLogin() 
        {
            error = await bl.dto.Users.UserLogin(dto.Username, dto.Password);

            if (!string.IsNullOrEmpty(error))
            {
                TempData[bl.refs.ErrorMessageLogin] = error;
                return RedirectToPage();
            }

            usr = await bl.model.Users.CheackUserHave(dto.Username);

            var _ps = new _session();

            _ps.UserLogin(HttpContext, usr);

            var admin = _ps.IsAddminUser(HttpContext, usr.Role);
            if(!admin) 
            {
                TempData[bl.refs.ErrorMessageLogin] = "Not admin";
                _ps.ClearSession(HttpContext);
                return RedirectToPage();
            }
            TempData[bl.refs.SeccessMessage] = $@"successfully Login {dto.Role}";

            bl.sys.Acceslog("Accees", _ps.GetSessionValue(HttpContext, "_FullName"), "Login");

            return RedirectToPage("/PCPMS/Dashboard/Index");

        }
    }
}
