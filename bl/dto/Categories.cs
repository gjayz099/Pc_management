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

            int count = await bl.data.Categories.CheckCountCategories(categories.CategoryName);

            if (count > 0) return "Category Name is exists";

            await bl.data.Categories.InsertRequestAsync(categories);

            return "";
        }
    }
}
