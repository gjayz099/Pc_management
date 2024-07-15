namespace bl.data
{
    public class Sales
    {

        public static string sqlSelectAll = $@"
	            SELECT 
                    sle.Id
                    ,mft.ManufatureName
                    ,ctg.CategoriesName
	                ,mft.Specification
                    ,sle.Quantity_Sold
                    ,sle.UnitPrice
                    ,sle.Total_Price
                    ,CONCAT(usr.Firstname, '-', usr.Lastname)
                    ,sle.Date_sale
                FROM gerald_pcpms_db.dbo.pcpms_sale AS sle
                JOIN gerald_pcpms_db.dbo.pcpms_User AS usr ON usr.Id = sle.CustomerID
                JOIN gerald_pcpms_db.dbo.pcpms_manufature AS mft ON mft.Id = sle.PartID
                LEFT JOIN gerald_pcpms_db.dbo.pcpms_categories AS ctg ON ctg.Id = mft.CategoryID;";

        public static async Task<(bool err, string stauts, List<bl.model.Sales.SaleWithCustomer> data)> ExecuteQueryAsync()
		{
			var ret = await bl.DBaccess.OldRawSqlQueryAsync(sqlSelectAll, x => new bl.model.Sales.SaleWithCustomer
            {
                Id = (x[0] == DBNull.Value) ? Guid.Empty : (Guid)x[0],
                Manufature = (x[1] == DBNull.Value) ? string.Empty : (string)x[1],
                Category = (x[2] == DBNull.Value) ? string.Empty : (string)x[2],
                Specification = (x[3] == DBNull.Value) ? string.Empty : (string)x[3],
                QuantitySold = (x[4] == DBNull.Value) ? 0 : (int)x[4],
                UnitPrice = (x[5] == DBNull.Value) ? 0 : (decimal)x[5],
                TotalPrice = (x[6] == DBNull.Value) ? 0 : (decimal)x[6],
                CustomerFullname = (x[7] == DBNull.Value) ? string.Empty : (string)x[7],
                DateSale = (x[8] == DBNull.Value) ? DateTime.MinValue : (DateTime)x[8]
            });


            return (false, "Success", ret);

        }
    }

}
