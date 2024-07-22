using bl.model;

namespace bl.data
{
    public class Customer
    {
        static string sqlSelecbuyItem = $@"
                    SELECT
			             sll.Id
			             ,mft.ManufatureName
			             ,CONCAT(mft.ManufatureName, ' ',mft.Specification)
			             ,mft.Price
			             ,sll.Quantity_Sold
			             ,sll.Total_Price
                         ,ctg.CategoriesName    
                    FROM gerald_pcpms_db.dbo.pcpms_customer cus
		            JOIN gerald_pcpms_db.dbo.pcpms_sale sll ON sll.CustomerId = cus.Id
		            RIGHT JOIN gerald_pcpms_db.dbo.pcpms_manufature mft ON mft.Id = sll.PartID
                    RIGHT JOIN gerald_pcpms_db.dbo.pcpms_categories ctg ON ctg.Id = mft.CategoryID";



        public static async Task<(bool err, string stauts, List<bl.model.Customer.CusToTotalBuyItme> data)>BuyItemExecuteQueryAsync(Guid Id)
        {
            string sqlSelectAll = $@"{sqlSelecbuyItem} WHERE cus.Id = '{Id}'";


            var ret = await bl.DBaccess.OldRawSqlQueryAsync(sqlSelectAll, x => new bl.model.Customer.CusToTotalBuyItme
            {
                SellId = (x[0] == DBNull.Value) ? Guid.Empty : (Guid)x[0],
                ManuName = (x[1] == DBNull.Value) ? string.Empty : (string)x[1],
                ItemName = (x[2] == DBNull.Value) ? string.Empty : (string)x[2],
                TotalPrice = (x[3] == DBNull.Value) ? 0 : (decimal)x[3],
                ItemSold = (x[4] == DBNull.Value) ? 0 : (int)x[4],
                TotalPriceItem = (x[5] == DBNull.Value) ? 0 : (decimal)x[5],
                Category = (x[6] == DBNull.Value) ? string.Empty : (string)x[6],

            });

            return (false, "Success", ret);
        }


        public static async Task<bl.model.Customer.CusToTotalBuy> ExecuteQueryCutomerBuyAsync(Guid Id)
        {
            string sqlCutomertobuy = $@" SELECT
                                cus.Id
                                ,cus.Firstname
                                ,cus.Lastname
                               ,Sum(sl.Total_Price)
                            FROM {bl.refs.Databse_DB}.dbo.pcpms_customer cus
                             JOIN {bl.refs.Databse_DB}.dbo.pcpms_sale sl on sl.CustomerId = cus.Id
                             WHERE cus.Id = @Id
							 GROUP BY cus.Id, cus.Firstname, cus.Lastname";


            var par = new List<Microsoft.Data.SqlClient.SqlParameter>
            {
                new Microsoft.Data.SqlClient.SqlParameter{ ParameterName = "@Id", Value = Id}
            };

            var ret = await bl.DBaccess.RawSqlQuerySingleAsync(sqlCutomertobuy, par, x => new bl.model.Customer.CusToTotalBuy
            {
                Id = (x[0] == DBNull.Value) ? Guid.Empty : (Guid)x[0],
                Firstname = (x[1] == DBNull.Value) ? string.Empty : (string)x[1],
                Lastname = (x[2] == DBNull.Value) ? string.Empty : (string)x[2],
                TotalPriceItem = (x[3] == DBNull.Value) ? 0 : (decimal)x[3],
            });

            return ret.data;

        }

        public static async Task<List<bl.model.Customer.CusToTotalBuy>> ExecuteQueryAsync()
        {
            string sqlSelectAll = $@"
                SELECT
                   cus.Id
                   ,cus.Firstname
                   ,cus.Lastname
		           ,Sum(sl.Total_Price)
                FROM {bl.refs.Databse_DB}.dbo.pcpms_customer cus
                JOIN {bl.refs.Databse_DB}.dbo.pcpms_sale sl on sl.CustomerId = cus.Id
                GROUP BY cus.Id, cus.Firstname, cus.Lastname;";


            var ret = await bl.DBaccess.RawSqlQueryAsync(sqlSelectAll, x => new bl.model.Customer.CusToTotalBuy
            {
                Id = (x[0] == DBNull.Value) ? Guid.Empty : (Guid)x[0],
                Firstname = (x[1] == DBNull.Value) ? string.Empty : (string)x[1],
                Lastname = (x[2] == DBNull.Value) ? string.Empty : (string)x[2],
                TotalPriceItem = (x[3] == DBNull.Value) ? 0 : (decimal)x[3],
                
            });

            return ret.data;
        }


        public static async Task<bl.model.Customer> ExecuteQueryNameAsync(string firstname)
        {
                string sqlName = $@"   SELECT
                        Id
                        ,Firstname
                        ,Lastname
                        FROM {bl.refs.Databse_DB}.dbo.pcpms_customer
                        WHERE Firstname = @Firstname";


            var par = new List<Microsoft.Data.SqlClient.SqlParameter>
            {
                new Microsoft.Data.SqlClient.SqlParameter{ ParameterName = "@Firstname", Value = firstname}
            };

            var ret = await bl.DBaccess.RawSqlQuerySingleAsync(sqlName, par, x => new bl.model.Customer
            {
                Id = (x[0] == DBNull.Value) ? Guid.Empty : (Guid)x[0],
                Firstname = (x[1] == DBNull.Value) ? string.Empty : (string)x[1],
                Lastname = (x[2] == DBNull.Value) ? string.Empty : (string)x[2],
            });

            return ret.data;
        }

        public static async Task<string> InsertRequestAsync(bl.dto.Customer dto)
        {
            string SqlInsert = $@"
              INSERT INTO {bl.refs.Databse_DB}.dbo.pcpms_customer
                (
                    Id
                    ,Firstname
                    ,Lastname
                )
                    VALUES
                (
                    NEWID()
                    ,@Firstname
                    ,@Lastname
                )
            ";

            var ret = await bl.DBaccess.ExecNonQueryAsync(SqlInsert, new List<Microsoft.Data.SqlClient.SqlParameter>
            {
                new Microsoft.Data.SqlClient.SqlParameter{ ParameterName = "@Firstname", Value = dto.Firstname  },
                new Microsoft.Data.SqlClient.SqlParameter{ ParameterName = "@Lastname", Value = dto.Lastname  }
            });


            return $"Add {ret} Data";
        }



        public static async Task<string> DeleteRequestAsync(string Firstname)
        {
            string SqlInsert = $@"
              DELETE FROM {bl.refs.Databse_DB}.dbo.pcpms_customer
              WHERE Firstname = @Firstname";

            var ret = await bl.DBaccess.ExecNonQueryAsync(SqlInsert, new List<Microsoft.Data.SqlClient.SqlParameter>
            {
                new Microsoft.Data.SqlClient.SqlParameter{ ParameterName = "@Firstname", Value = Firstname  },
            });


            return $"Add {ret} Data";
        }


        public static async Task<int> CountCustomerExecuteQueryAsync()
        {
            string sqlSelectAll = $@"SELECT 
                                    COUNT(*)
                                    FROM {bl.refs.Databse_DB}.dbo.pcpms_customer";

            int ret = await bl.DBaccess.ExecuteScalarAsync(sqlSelectAll);

            return ret;
        }
    }
}
