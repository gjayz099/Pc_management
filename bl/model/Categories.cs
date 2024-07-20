namespace bl.model
{
    public class Categories
    {
        public Guid Id { get; set; }
        public string CategoryName { get; set; } // Motherboard

        public static async Task<List<bl.model.Categories>> GetAllAsync()
        {

            var ret = await bl.data.Categories.ExecuteQueryAsync();

            return ret;

        }

    }
}
