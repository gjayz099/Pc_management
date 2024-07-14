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


        public static async Task<int> InsertRequestAsync(bl.dto.Categories dto)
        {
            string Sql = $@"
                INSERT INTO 
	                {bl.refs.Databse_DB}.dbo.pcpms_categories 
	            (
                    Id
                   ,CategoiesName)
	                VALUES 
	            (
                    NEWID()
                   ,@CategoiesName
                );";

            var ret = await bl.DBaccess.OldExecNonQueryAsync(Sql, new List<Microsoft.Data.SqlClient.SqlParameter>
            {
                new Microsoft.Data.SqlClient.SqlParameter{ ParameterName = "@CategoiesName", Value = dto.CategoryName  }
            });

            return ret;
        }

        //Checks if a category exists by its name asynchronously
        //public static async Task<(bool status, string err, List<bool> ret)> CheckIfExistingByCategories(string branchcode)
        //{
        //    string Sql = $@"
        //       SELECT CASE WHEN EXISTS (
        //        SELECT 1
        //        FROM {bl.refs.Databse_DB}.dbo.pcpms_categories
        //        WHERE CategoiesName = @CategoiesName
        //    ) THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END AS result";

        //    var ret = await bl.DBaccess.RawSqlQueryAsync<bool>(Sql, new List<Microsoft.Data.SqlClient.SqlParameter>
        //    {
        //        new Microsoft.Data.SqlClient.SqlParameter { ParameterName = "@CategoiesName", Value = branchcode }
        //    }, x =>
        //    {
        //        return (x[0] == DBNull.Value) ? false : Convert.ToBoolean(x[0]);
        //    });

        //    return ret;
        //}



        public static async Task<int> CheckCountCategories(string branchcode)
        {
            string sqlQuery = $@"
                SELECT COUNT(*)
                FROM {bl.refs.Databse_DB}.dbo.pcpms_categories
                where CategoiesName =@CategoiesName"
                ;

            var ret = new List<Microsoft.Data.SqlClient.SqlParameter>
            {
                new Microsoft.Data.SqlClient.SqlParameter { ParameterName = "@CategoiesName", Value = branchcode }
            };

            int Count = await bl.DBaccess.ExecuteScalarAsync(sqlQuery, ret); 

            return Count;
        }
    }
}
