using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PcPartManagementSystems.Pages.PCPMS.Data.Manufature
{
    public class EditModel : PageModel
    {
        [BindProperty(SupportsGet = true)] public Guid? Id { get; set; }

        [BindProperty] public bl.dto.Manufacturies dto { get; set; }

        [BindProperty] public string error { get; set; }

        [BindProperty] public IFormFile PictureFile { get; set; }

        public async Task<IActionResult> OnGet()
        {

            var _ps = new _session();
            if (!_ps.IsUserLoggedIn(HttpContext)) { return RedirectToPage("/Index"); }

            if (Id == null || Id == Guid.Empty) {return Page(); }

            dto = await bl.model.Manufacturies.GetIDAsync(Id);


            return Page();
        }

        public async Task<IActionResult> OnPostSaveData()
        {

            if (Id != null && Id != Guid.Empty)
            {
                error = await bl.dto.Manufacturies.UpdateData(dto, Id.Value);

                if (!string.IsNullOrEmpty(error))
                {
                    TempData[bl.refs.ErrorMessage] = error;
                    return Page();
                }
                TempData[bl.refs.SeccessMessage] = $@"{dto.ManufactureName} successfully Update!";
            }
            else
            {
                error = await bl.dto.Manufacturies.InsertData(dto, PictureFile);

                if (!string.IsNullOrEmpty(error))
                {
                    TempData[bl.refs.ErrorMessage] = error;
                    return Page();
                }
                TempData[bl.refs.SeccessMessage] = $@"{dto.ManufactureName} successfully inserted!";
            }

            return RedirectToPage("/PCPMS/Data/Manufature/Index");




 
        }

    }
}
