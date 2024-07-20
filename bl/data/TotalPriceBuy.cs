namespace bl.data
{
    public class TotalPriceBuy
    {
        public static async Task<string> InsertRequestAsync(bl.dto.TotalPriceBuy dto, Guid cus)
        {
            string SqlInsert = $@"
              INSERT INTO {bl.refs.Databse_DB}.dbo.pcpms_totalPrice
                (
                    Id
                    ,TotalPrice
                    ,CustomerID
                )
                    VALUES
                (
                    NEWID()
                    ,@TotalPrice
                    ,@CustomerID
                )
            ";

            var ret = await bl.DBaccess.ExecNonQueryAsync(SqlInsert, new List<Microsoft.Data.SqlClient.SqlParameter>
            {
                new Microsoft.Data.SqlClient.SqlParameter{ ParameterName = "@TotalPrice", Value = dto.TotalPrice },
                new Microsoft.Data.SqlClient.SqlParameter{ ParameterName = "@CustomerID", Value = cus}
            });


            return $"Add {ret} Data";
        }
    }
}
