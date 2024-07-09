namespace bl.data
{
    public class Categories
    {
        // SQL query string to select all categories' Id and CategoryName from pcpms_categories table
        public static string sqlSelectAll = $@"
                            SELECT
                                Id
                                ,CategoiesName
                            FROM {bl.refs.Databse_DB}.dbo.pcpms_categories";

        // Executes a SQL query asynchronously and maps the results to a list of Category objects
        public static async Task<List<bl.model.Categories>> ExecuteQueryAsync(string sql)
        {
            var ret = await bl.DBaccess.RawSqlQueryAsync(sql, x => new bl.model.Categories
            {
                // Maps the first column (Id) to Category.Id and handles DBNull values
                Id = (x[0] == DBNull.Value) ? Guid.Empty : (Guid)x[0],
                // Maps the second column (CategoiesName) to Category.CategoryName and handles DBNull values
                CategoryName = (x[1] == DBNull.Value) ? string.Empty : (string)x[1]
            });

            return ret.data; // Returns the list of Category objects
        }



        // Checks if a category exists by its name asynchronously
        public static async Task<(bool status, string err, List<bool> ret)> CheckIfExistingByCategories(string branchcode)
        {
            string Sql = $@"
            SELECT ISNULL(
                (SELECT 1 
                 FROM {bl.refs.Databse_DB}.dbo.pcpms_categories
                 WHERE CategoiesName = @CategoiesName)
                ,0
            ) AS result";

            var ret = await bl.DBaccess.RawSqlQueryAsync<bool>(Sql, new List<Microsoft.Data.SqlClient.SqlParameter>
            {
                new Microsoft.Data.SqlClient.SqlParameter("@CategoiesName", branchcode) // Parameterized query to check existence by category name
            }, x =>
            {
                return (x[0] == DBNull.Value) ? false : Convert.ToBoolean(x[0]); // Converts the query result to a boolean indicating existence
            });

            return ret; // Returns the status (true if exists, false otherwise)
        }




        //public async Task<int> GetTotalCategoriesCountAsync()
        //{
        //    string sqlQuery = @"
        //SELECT COUNT(*)
        //FROM dbo.pcpms_categories";

        //    int totalCount = await YourClassName.getInt(sqlQuery); // Replace YourClassName with your actual class name

          //return totalCount;
        //}
    }
}
