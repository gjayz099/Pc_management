namespace bl.data
{
    public class Parts
    {


        public static async Task<List<bl.model.Parts.PartManufacture>> ExecuteQueryAsync()
        {
            // SQL query to select all parts from the database
            string sqlSelectAll = $@"
                SELECT
                    prt.id
                    ,mtr.ManufatureName
                    ,mtr.Specification
                    ,ctr.CategoiesName
	                ,mtr.Price
	                ,mtr.Stock
	                ,prt.Description
            FROM {bl.refs.Databse_DB}.dbo.pcpms_parts prt
            JOIN {bl.refs.Databse_DB}.dbo.pcpms_manufature mtr ON prt.ManufactureID = mtr.Id
            LEFT JOIN {bl.refs.Databse_DB}.dbo.pcpms_categories ctr ON mtr.CategoryID = ctr.Id;";

            // Execute the SQL query asynchronously and map the results to Parts model
            var ret = await bl.DBaccess.RawSqlQueryAsync(sqlSelectAll, x => new bl.model.Parts.PartManufacture
            {
                Id = (x[0] == DBNull.Value) ? Guid.Empty : (Guid)x[0],
                ManufactureName = (x[1] == DBNull.Value) ? string.Empty : (string)x[1],
                Specifation = (x[2] == DBNull.Value) ? string.Empty : (string)x[2],
                CategoreName = (x[3] == DBNull.Value) ? string.Empty : (string)x[3],
                Price = (x[4] == DBNull.Value) ? 0 : (decimal)x[4],
                Stock = (x[5] == DBNull.Value) ? 0 : (int)x[5],
                Description = (x[6] == DBNull.Value) ? string.Empty : (string)x[6],
            });

            // Return the list of Parts retrieved from the database
            return ret.data;
        }



    }
}
