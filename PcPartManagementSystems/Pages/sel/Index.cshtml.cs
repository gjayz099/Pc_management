using bl.model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace PcPartManagementSystems.Pages.sel
{
    public class IndexModel : PageModel
    {

        public List<Sel> sel2 { get; set; } // Use correct model type
        public IActionResult OnGet()
        {
            return Page();
        }


        public async Task<IActionResult> OnGetAllCategory()
        {


            var categories = await bl.model.Categories.GetAllAsync();

            // Convert fetched categories to List<Sel>
            sel2 = categories.ConvertAll(c => new bl.model.Sel
            {
                Id = c.Id.ToString(),         // Convert Guid to string if needed
                text = c.CategoryName,
              
            });

            return new JsonResult(sel2);
        }
    }
}
