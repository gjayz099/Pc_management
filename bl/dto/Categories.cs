namespace bl.dto
{
    public class Categories
    {
        public string CategoryName { get; set; }


        public string Validate()
        {
            if (string.IsNullOrEmpty(CategoryName)) return "Category Name is empty";

            return ""; 
        }

        public static async Task<List<bl.model.Categories>> GetAllAsync()
        {

            List<bl.model.Categories> ret;
            ret =  await bl.data.Categories.ExecuteQueryAsync();

            return ret;
          
        }
    }
}
