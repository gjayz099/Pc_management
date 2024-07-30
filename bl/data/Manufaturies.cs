namespace bl.data
{
    public class Manufaturies
    {

        public static string  sqlSelectAll = $@"
                SELECT
                    mtr.id
                    ,mtr.ManufatureName
                    ,mtr.Specification
                    ,ctr.CategoriesName
	                ,mtr.Price
	                ,mtr.Stock
	                ,mtr.Description
                    ,mtr.PictureName
                    FROM {bl.refs.Databse_DB}.dbo.pcpms_manufature mtr
                    JOIN {bl.refs.Databse_DB}.dbo.pcpms_categories ctr ON ctr.Id = mtr.CategoryID";

        public static async Task<(bool err, string stauts, List<bl.model.Manufacturies.ManufacturiesWithCategories> data)> ExecuteQueryAsync()
        {
            // SQL query to select all manufacturers with categories
         

            // Execute the raw SQL query asynchronously
            var ret  = await bl.DBaccess.OldRawSqlQueryAsync(sqlSelectAll, x => new bl.model.Manufacturies.ManufacturiesWithCategories
            {
                // Mapping SQL result to C# object
                Id = (x[0] == DBNull.Value) ? Guid.Empty : (Guid)x[0],
                ManufactureName = (x[1] == DBNull.Value) ? string.Empty : (string)x[1],
                Specification = (x[2] == DBNull.Value) ? string.Empty : (string)x[2],
                CategotyName = (x[3] == DBNull.Value) ? string.Empty : (string)x[3],
                Price = (x[4] == DBNull.Value) ? 0 : (decimal)x[4],
                Stock = (x[5] == DBNull.Value) ? 0 : (int)x[5],
                Description = (x[6] == DBNull.Value) ? string.Empty : (string)x[6],
                PictureName = (x[7] == DBNull.Value) ? string.Empty : (string)x[7],
            });

            // Return the list of manufacturers with categories
            return (false, "Success", ret);
        }





        public static async Task<bl.dto.Manufacturies> ExecuteQueryIDAsync(Guid? id)
        {
                    string sqlwhereID = $@"
                    SELECT
                        ManufatureName
                        ,Specification
                        ,CategoryID
                        ,Price
                        ,Stock
                        ,Description
                        ,PictureName
                    FROM {bl.refs.Databse_DB}.dbo.pcpms_manufature
                    WHERE Id = @Id";
            var par = new List<Microsoft.Data.SqlClient.SqlParameter>
            {
                new Microsoft.Data.SqlClient.SqlParameter{ ParameterName = "@Id", Value = id}
            };

            var ret = await bl.DBaccess.RawSqlQuerySingleAsync(sqlwhereID, par, x => new bl.dto.Manufacturies
            {

   
                ManufactureName = (x[0] == DBNull.Value) ? string.Empty : (string)x[0],
                Specification = (x[1] == DBNull.Value) ? string.Empty : (string)x[1],
                CategotyID = (x[2] == DBNull.Value) ? Guid.Empty : (Guid)x[2],
                Price = (x[3] == DBNull.Value) ? 0 : (decimal)x[3],
                Stock = (x[4] == DBNull.Value) ? 0 : (int)x[4],
                Description = (x[5] == DBNull.Value) ? string.Empty : (string)x[5],
                PictureName = (x[6] == DBNull.Value) ? string.Empty : (string)x[6],
     
            });


            return ret.data;

        }


        public static async Task<int> InsertDataAsync(bl.dto.Manufacturies dto, string pictureFileName)
        {
            string sql = $@"INSERT INTO {bl.refs.Databse_DB}.dbo.pcpms_manufature
                            (    
                             Id
                             ,ManufatureName
                             ,Specification
                             ,CategoryID
                             ,Price
                             ,Stock
                             ,Description
                             ,PictureName
                            )
                            VALUES
                            (
                             NEWID()
                             ,@ManufatureName
                             ,@Specification
                             ,@CategoryID
                             ,@Price
                             ,@Stock
                             ,@Description
                             ,@PictureName
                            )";

            var ret = await bl.DBaccess.OldExecNonQueryAsync(sql, new List<Microsoft.Data.SqlClient.SqlParameter>
            {
                new Microsoft.Data.SqlClient.SqlParameter{ ParameterName = "@ManufatureName", Value = dto.ManufactureName},
                new Microsoft.Data.SqlClient.SqlParameter{ ParameterName = "@Specification", Value = dto.Specification},
                new Microsoft.Data.SqlClient.SqlParameter{ ParameterName = "@CategoryID", Value = dto.CategotyID},
                new Microsoft.Data.SqlClient.SqlParameter{ ParameterName = "@Price", Value = dto.Price},
                new Microsoft.Data.SqlClient.SqlParameter{ ParameterName = "@Stock", Value = dto.Stock},
                new Microsoft.Data.SqlClient.SqlParameter{ ParameterName = "@Description", Value = string.IsNullOrEmpty(dto.Description)? DBNull.Value : dto.Description},
                new Microsoft.Data.SqlClient.SqlParameter{ ParameterName = "@PictureName", Value = pictureFileName}
            });


            return ret;
        }


        public static async Task<int> UpdateDataAsync(bl.dto.Manufacturies dto, Guid id)
        {
            string sql = $@"
                        UPDATE {refs.Databse_DB}.dbo.pcpms_manufature SET 
                         ManufatureName = @ManufatureName
                         ,CategoryID = @CategoryID
                         ,Stock =@Stock
                         ,Price = @Price
                         ,Specification = @Specification
                         ,Description = @Description
                         where Id = '{id}'";


            var ret = await bl.DBaccess.OldExecNonQueryAsync(sql, new List<Microsoft.Data.SqlClient.SqlParameter>
            {
                new Microsoft.Data.SqlClient.SqlParameter{ ParameterName = "@ManufatureName", Value = dto.ManufactureName},
                new Microsoft.Data.SqlClient.SqlParameter{ ParameterName = "@Specification", Value = dto.Specification},
                new Microsoft.Data.SqlClient.SqlParameter{ ParameterName = "@CategoryID", Value = dto.CategotyID},
                new Microsoft.Data.SqlClient.SqlParameter{ ParameterName = "@Price", Value = dto.Price},
                new Microsoft.Data.SqlClient.SqlParameter{ ParameterName = "@Stock", Value = dto.Stock},
                new Microsoft.Data.SqlClient.SqlParameter{ ParameterName = "@Description", Value = string.IsNullOrEmpty(dto.Description)? DBNull.Value : dto.Description},
            });

        


            return ret;
        }

        public static async Task<int> CountManuExecuteQueryAsync()
        {
            string sqlSelectAll = $@"SELECT 
                                    COUNT(*)
                                    FROM {bl.refs.Databse_DB}.dbo.pcpms_manufature";

            int ret = await bl.DBaccess.ExecuteScalarAsync(sqlSelectAll);

            return ret;
        }

        //-- Example: Report for products in a specific category with additional details
        public static async Task<List<bl.report.PSC>> RCSExecuteQueryAsync(Guid CategoryID)
        {

            string RPSsql = $@"
                SELECT 
                    c.CategoriesName 
                    ,m.ManufatureName
                    ,m.Stock
                    ,m.Price
                    ,m.Specification
                    ,ISNULL(m.Description, '')
                FROM pcpms_manufature m
                JOIN pcpms_categories c ON m.CategoryID = c.Id
                {(CategoryID != Guid.Empty ? $" WHERE m.CategoryID = '{CategoryID}'" : "")}";
  


          

            var ret = await bl.DBaccess.RawSqlQueryAsync(RPSsql, x => new bl.report.PSC
            {

                CatigoriesName = (x[0] == DBNull.Value) ? string.Empty : (string)x[0],
                ManufatureName = (x[1] == DBNull.Value) ? string.Empty : (string)x[1],
                Stock = (x[2] == DBNull.Value) ? 0 : (int)x[2],
                Price = (x[3] == DBNull.Value) ? 0 : (decimal)x[3],
                Specification = (x[4] == DBNull.Value) ? string.Empty : (string)x[4],
                Description = (x[5] == DBNull.Value) ? string.Empty : (string)x[5],
            });

            return ret.data;
        }

        // -- Example: Report for products in a specific category with stock below a certain threshold
        public static async Task<List<bl.report.PSCSB>> RPSCSBExecuteQueryAsync(Guid CategoryID, int stock)
        {
            string RPSCSBEsql = $@"
                SELECT 
                    c.CategoriesName AS CategoryName
                    ,m.ManufatureName AS ProductName
                    ,m.Stock
                FROM pcpms_manufature m
                JOIN pcpms_categories c ON m.CategoryID = c.Id
                WHERE  m.Stock < {stock}  
                {(CategoryID != Guid.Empty ? $"AND m.CategoriesName  = '{CategoryID}'" : "")}
                ORDER BY m.ManufatureName";

            var ret = await bl.DBaccess.RawSqlQueryAsync(RPSCSBEsql, x => new bl.report.PSCSB
            {
                CategoryName = (x[0] == DBNull.Value) ? string.Empty : (string)x[0],
                ManufacturerName = (x[1] == DBNull.Value) ? string.Empty : (string)x[1],
                Stock = (x[2] == DBNull.Value) ? 0 : (int)x[2]
            });

            return ret.data;

        }
    }
}
