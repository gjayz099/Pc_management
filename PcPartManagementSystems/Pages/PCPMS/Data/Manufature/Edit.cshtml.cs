using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;

namespace PcPartManagementSystems.Pages.PCPMS.Data.Manufature
{
    public class EditModel : PageModel
    {
        [BindProperty(SupportsGet = true)] public Guid? Id { get; set; }

        [BindProperty] public bl.dto.Manufacturies dto { get; set; }

        [BindProperty] public string error { get; set; }

        public async Task<IActionResult> OnGet()
        {
            if (Id == Guid.Empty)
            {
                return Page();
            }

            dto = await bl.model.Manufacturies.GetIDAsync(Id);


            return Page();
        }

        //public async Task<IActionResult> OnPostSaveData()
        //{
         
        //    if (Id != Guid.Empty)
        //    {
        //        error = await bl.dto.Manufacturies.UpdateData(dto, Id);
        //        if (!string.IsNullOrEmpty(error))
        //        {
        //            TempData[bl.refs.ErrorMessage] = error;
        //            return RedirectToPage();
        //        }
        //    }
        //    else
        //    {
        //        error = await bl.dto.Manufacturies.InsertData(dto);
              
        //    }

      

        //    return RedirectToPage("/PCPMS/Data/Manufature/Index"); // Redirect to the list page after successful save
        //}

    }
}
