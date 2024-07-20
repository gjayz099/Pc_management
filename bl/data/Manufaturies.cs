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
                        ManufatureName,
                        Specification,
                        CategoryID,
                        Price,
                        Stock,
                        Description
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
     
            });


            return ret.data;

        }


        public static async Task<string> InsertDataAsync(bl.dto.Manufacturies dto, string pictureFileName)
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

            var ret = await bl.DBaccess.ExecNonQueryAsync(sql, new List<Microsoft.Data.SqlClient.SqlParameter>
            {
                new Microsoft.Data.SqlClient.SqlParameter{ ParameterName = "@ManufatureName", Value = dto.ManufactureName},
                new Microsoft.Data.SqlClient.SqlParameter{ ParameterName = "@Specification", Value = dto.Specification},
                new Microsoft.Data.SqlClient.SqlParameter{ ParameterName = "@CategoryID", Value = dto.CategotyID},
                new Microsoft.Data.SqlClient.SqlParameter{ ParameterName = "@Price", Value = dto.Price},
                new Microsoft.Data.SqlClient.SqlParameter{ ParameterName = "@Stock", Value = dto.Stock},
                new Microsoft.Data.SqlClient.SqlParameter{ ParameterName = "@Description", Value = (object)dto.Description ??  DBNull.Value},
                new Microsoft.Data.SqlClient.SqlParameter{ ParameterName = "@PictureName", Value = pictureFileName}
            });


            return $"Succes To Insert {ret} Data {dto.ManufactureName} Manufature";
        }


        public static async Task<string> UpdateDataAsync(bl.dto.Manufacturies dto, Guid id)
        {
            string sql = $@"
                        UPDATE gerald_pcpms_db.dbo.pcpms_manufature SET 
                         ManufatureName = @ManufatureName
                         ,CategoryID = @CategoryID
                         ,Stock =@Stock
                         ,Price = @Price
                         ,Specification = @Specification
                         ,Description = @Description
                         where Id = '{id}'";


            var ret = await bl.DBaccess.ExecNonQueryAsync(sql, new List<Microsoft.Data.SqlClient.SqlParameter>
            {
                new Microsoft.Data.SqlClient.SqlParameter{ ParameterName = "@ManufatureName", Value = dto.ManufactureName},
                new Microsoft.Data.SqlClient.SqlParameter{ ParameterName = "@Specification", Value = dto.Specification},
                new Microsoft.Data.SqlClient.SqlParameter{ ParameterName = "@CategoryID", Value = dto.CategotyID},
                new Microsoft.Data.SqlClient.SqlParameter{ ParameterName = "@Price", Value = dto.Price},
                new Microsoft.Data.SqlClient.SqlParameter{ ParameterName = "@Stock", Value = dto.Stock},
                new Microsoft.Data.SqlClient.SqlParameter{ ParameterName = "@Description", Value = (object)dto.Description ??  DBNull.Value}
            });

        


            return $"Succes To Update {ret} Data {dto.ManufactureName} Manufature";
        }


    }
}
