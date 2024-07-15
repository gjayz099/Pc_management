namespace bl.data
{
    public class Categories
    {


        // Executes a SQL query asynchronously and maps the results to a list of Category objects
        public static async Task<List<bl.model.Categories>> ExecuteQueryAsync()
        {
             // SQL query string to select all categories' Id and CategoryName from pcpms_categories table
             string sqlSelectAll = $@"
                SELECT
                    Id
                    ,CategoriesName
                FROM {bl.refs.Databse_DB}.dbo.pcpms_categories";

            var ret = await bl.DBaccess.RawSqlQueryAsync(sqlSelectAll, x => new bl.model.Categories
            {
                // Maps the first column (Id) to Category.Id and handles DBNull values
                Id = (x[0] == DBNull.Value) ? Guid.Empty : (Guid)x[0],
                // Maps the second column (CategoiesName) to Category.CategoryName and handles DBNull values
                CategoryName = (x[1] == DBNull.Value) ? string.Empty : (string)x[1]
            });

            return ret.data; // Returns the list of Category objects
        }


        public static async Task<string> InsertRequestAsync(bl.dto.Categories dto)
        {
            string Sql = $@"
                INSERT INTO 
	                {bl.refs.Databse_DB}.dbo.pcpms_categories 
	            (
                    Id
                   ,CategoriesName)
	                VALUES 
	            (
                    NEWID()
                   ,@CategoriesName
                )";

            var ret = await bl.DBaccess.OldExecNonQueryAsync(Sql, new List<Microsoft.Data.SqlClient.SqlParameter>
            {
                new Microsoft.Data.SqlClient.SqlParameter{ ParameterName = "@CategoriesName", Value = dto.CategoryName  }
            });

            return $"Add {ret} Data {dto.CategoryName} Category";
        }



        public static async Task<int> CheckCountCategories(string branchcode)
        {
            string sqlQuery = $@"
                SELECT COUNT(*)
                FROM {bl.refs.Databse_DB}.dbo.pcpms_categories
                where CategoriesName = @CategoriesName"
                ;

            var ret = new List<Microsoft.Data.SqlClient.SqlParameter>
            {
                new Microsoft.Data.SqlClient.SqlParameter { ParameterName = "@CategoriesName", Value = branchcode }
            };

            int Count = await bl.DBaccess.ExecuteScalarAsync(sqlQuery, ret); 

            return Count;
        }
    }
}
