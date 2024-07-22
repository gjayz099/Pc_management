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
                    ,CONCAT(ct.Firstname,' ',ct.Lastname)
                    ,sle.Date_sale
                FROM {bl.refs.Databse_DB}.dbo.pcpms_sale AS sle
                JOIN {bl.refs.Databse_DB}.dbo.pcpms_manufature AS mft ON mft.Id = sle.PartID
                JOIN {bl.refs.Databse_DB}.dbo.pcpms_customer AS ct ON ct.Id = sle.CustomerId
                LEFT JOIN {bl.refs.Databse_DB}.dbo.pcpms_categories AS ctg ON ctg.Id = mft.CategoryID;";

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

        public static async Task<string> InsertRequestAsync(List<bl.dto.Sales> dtoList, Guid CusId)
        {
            string SqlInsert = $@"
                INSERT INTO {bl.refs.Databse_DB}.dbo.pcpms_sale
                (
                    Id 
                    ,PartID
                    ,Quantity_Sold
                    ,UnitPrice
                    ,CustomerId
                    ,Total_Price
                    ,Date_sale
                )
                    VALUES
                (
                    NEWID()
                    ,@PartID
                    ,@Quantity_Sold
                    ,@UnitPrice
                    ,@CustomerId
                    ,@Total_Price
                    ,@Date_sale
                )
            ";

            foreach (var dto in dtoList)
            {
                var ret = await bl.DBaccess.OldExecNonQueryAsync(SqlInsert, new List<Microsoft.Data.SqlClient.SqlParameter>
                    {
                        new Microsoft.Data.SqlClient.SqlParameter{ ParameterName = "@PartID", Value = dto.PartsID },
                        new Microsoft.Data.SqlClient.SqlParameter{ ParameterName = "@Quantity_Sold", Value = dto.QuantitySold  },
                        new Microsoft.Data.SqlClient.SqlParameter{ ParameterName = "@UnitPrice", Value = dto.UnitPrice  },
                        new Microsoft.Data.SqlClient.SqlParameter{ ParameterName = "@CustomerId", Value = CusId  },
                        new Microsoft.Data.SqlClient.SqlParameter{ ParameterName = "@Total_Price", Value = dto.TotalPrice },
                        new Microsoft.Data.SqlClient.SqlParameter{ ParameterName = "@Date_sale", Value = DateTime.Now  }
                    });

       

            }

            return $"Add {dtoList.Count} Data";

        }




        public static async Task<decimal> SumSaleExecuteQueryAsync()
        {
            string sqlSelectAll = $@"SELECT 
                                    Sum(Total_Price)
                                    FROM {bl.refs.Databse_DB}.dbo.pcpms_sale";

            decimal ret = await bl.DBaccess.ExecuteScalarAsync(sqlSelectAll);

            return ret;
        }

    }

}
