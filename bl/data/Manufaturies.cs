namespace bl.data
{
    public class Manufaturies
    {

        public static async Task<List<bl.model.Manufacturies.ManufacturiesWithCategories>> ExecuteQueryAsync()
        {
            // SQL query to select all manufacturers with categories
            string sqlSelectAll = $@"
                SELECT
                    mtr.id
                    ,mtr.ManufatureName
                    ,mtr.Specification
                    ,ctr.CategoiesName
	                ,mtr.Price
	                ,mtr.Stock
	                ,mtr.Description
                    FROM {bl.refs.Databse_DB}.dbo.pcpms_Manufature mtr
                    JOIN {bl.refs.Databse_DB}.dbo.pcpms_categories ctr ON ctr.Id = mtr.CategoryID;";

            // Execute the raw SQL query asynchronously
            var ret = await bl.DBaccess.RawSqlQueryAsync(sqlSelectAll, x => new bl.model.Manufacturies.ManufacturiesWithCategories
            {
                // Mapping SQL result to C# object
                Id = (x[0] == DBNull.Value) ? Guid.Empty : (Guid)x[0],
                ManufactureName = (x[1] == DBNull.Value) ? string.Empty : (string)x[1],
                Specification = (x[2] == DBNull.Value) ? string.Empty : (string)x[2],
                CategotyName = (x[3] == DBNull.Value) ? string.Empty : (string)x[3],
                Price = (x[4] == DBNull.Value) ? 0 : (decimal)x[4],
                Stock = (x[5] == DBNull.Value) ? 0 : (int)x[5],
                Description = (x[6] == DBNull.Value) ? string.Empty : (string)x[6],
            });

            // Return the list of manufacturers with categories
            return ret.data;
        }

        public static async Task<bl.model.Manufacturies.ManufacturiesWithCategories> ExecuteQueryIDAsync(Guid? id)
        {
            string sqlSelectAll = $@"
                SELECT
                    mtr.id
                    ,mtr.ManufatureName
                    ,mtr.Specification
                    ,ctr.CategoiesName
	                ,mtr.Price
	                ,mtr.Stock
	                ,mtr.Description
                    FROM {bl.refs.Databse_DB}.dbo.pcpms_Manufature mtr
                    JOIN {bl.refs.Databse_DB}.dbo.pcpms_categories ctr ON ctr.Id = mtr.CategoryID
                    WHERE mtr.id = @Id;";

            // Create parameter for Id
            var parameters = new List<Microsoft.Data.SqlClient.SqlParameter>
            {
                new Microsoft.Data.SqlClient.SqlParameter("@Id", id)
            };

            var ret = await bl.DBaccess.RawSqlQueryAsync(sqlSelectAll, parameters, x => new bl.model.Manufacturies.ManufacturiesWithCategories
            {
                Id = (x[0] == DBNull.Value) ? Guid.Empty : (Guid)x[0],
                ManufactureName = (x[1] == DBNull.Value) ? string.Empty : (string)x[1],
                Specification = (x[2] == DBNull.Value) ? string.Empty : (string)x[2],
                CategotyName = (x[3] == DBNull.Value) ? string.Empty : (string)x[3],
                Price = (x[4] == DBNull.Value) ? 0 : (decimal)x[4],
                Stock = (x[5] == DBNull.Value) ? 0 : (int)x[5],
                Description = (x[6] == DBNull.Value) ? string.Empty : (string)x[6],
            });

            return ret.data[0];
        }
    }
}
