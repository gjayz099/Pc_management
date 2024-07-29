using Syncfusion.XlsIO.Implementation.PivotAnalysis;
using System;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

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


        //-- Example: Report for sales of a specific product in a date range
        public static async Task<List<bl.report.PSC>> RPSExecuteQueryAsync(DateTime startDate, DateTime endDate, string categoryName = null)
        {

            string RPSsql = $@"SELECT 
                                s.Id AS SaleId,
                                c.Firstname AS CustomerFirstName,
                                c.Lastname AS CustomerLastName,
                                m.ManufatureName AS ProductName,
                                s.Quantity_Sold,
                                s.UnitPrice,
                                s.Total_Price,
                                s.Date_sale
                            FROM 
                                pcpms_sale s
                            JOIN 
                                pcpms_customer c ON s.CustomerId = c.Id
                            JOIN 
                                pcpms_manufature m ON s.PartID = m.Id
                            WHERE 
                                 s.Date_sale BETWEEN @StartDate AND @EndDate
                                {(string.IsNullOrEmpty(categoryName) ? "" : $"AND m.ManufatureName = @CategoryName")}
                            ORDER BY 
                                s.Date_sale DESC;";


            var par = new List<Microsoft.Data.SqlClient.SqlParameter>
            {
                new Microsoft.Data.SqlClient.SqlParameter{ ParameterName = "@StartDate", Value = DbType.DateTime},
                new Microsoft.Data.SqlClient.SqlParameter{ ParameterName = "@EndDate", Value = DbType.DateTime}

            };

            if (!string.IsNullOrEmpty(categoryName))
            {
                par.Add(new Microsoft.Data.SqlClient.SqlParameter("@CategoryName", categoryName));
            }

            var ret = await bl.DBaccess.RawSqlQueryAsync(sqlSelectAll, par, x => new bl.report.PSC
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
        //-- Example: Report for products sold in a specific date range
        public static async Task<List<bl.report.PSSD>> PSSDExecuteQueryAsync(DateTime startDate, DateTime endDate)
        {

     
            string RPSsql = $@"
                    DECLARE @StartDate DATETIME = '{startDate}'; -- Set your start date
                    DECLARE @EndDate DATETIME = '{endDate}'; -- Set your end date

                    SELECT
                        m.ManufatureName AS ProductName,
                        SUM(s.Quantity_Sold) AS TotalQuantitySold,
                        SUM(s.Total_Price) AS TotalSalesAmount
                    FROM
                        pcpms_sale s
                    JOIN
                        pcpms_manufature m ON s.PartID = m.Id
                    WHERE
                        s.Date_sale BETWEEN @StartDate AND @EndDate
                    GROUP BY
                        m.ManufatureName
                    ORDER BY
                        TotalSalesAmount DESC";



            var ret = await bl.DBaccess.RawSqlQueryAsync(RPSsql, x => new bl.report.PSSD
            {

                ManufatureName = (x[0] == DBNull.Value) ? string.Empty : (string)x[0],
                TotalQuantitySold = (x[1] == DBNull.Value) ? 0 : (int)x[1],
                TotalSalesAmount = (x[2] == DBNull.Value) ? 0 : (decimal)x[2],
            });

            return ret.data;
        }









    }


}
