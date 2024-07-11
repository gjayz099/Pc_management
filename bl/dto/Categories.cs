using Microsoft.IdentityModel.Tokens;

namespace bl.dto
{
    public class Categories
    {
        public string CategoryName { get; set; }


    
        public static async Task<List<bl.model.Categories>> GetAllAsync()
        {

            List<bl.model.Categories> ret;
            ret =  await bl.data.Categories.ExecuteQueryAsync();

            return ret;
          
        }

        public static async Task<string> InsertAllAsync(bl.dto.Categories categories)
        {
            if (string.IsNullOrEmpty(categories.CategoryName)) return "Category Name is empty";

            var existingCheck = await bl.data.Categories.CheckIfExistingByCategories(categories.CategoryName);

            if (existingCheck.status != true) return "Category Name already exists";

            await bl.data.Categories.InsertRequestAsync(categories);

            return "";
        }
    }
}
