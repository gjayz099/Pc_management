using bl.model;

namespace bl.data
{
    public class Customer
    {

        public static async Task<List<bl.model.Customer>> ExecuteQueryAsync()
        {
            string sqlSelectAll = $@"
                SELECT
                    cus.Id
                    ,cus.Firstname
                    ,cus.Lastname
                FROM {bl.refs.Databse_DB}.dbo.pcpms_customer cus";


            var ret = await bl.DBaccess.RawSqlQueryAsync(sqlSelectAll, x => new bl.model.Customer
            {
                Id = (x[0] == DBNull.Value) ? Guid.Empty : (Guid)x[0],
                Firstname = (x[1] == DBNull.Value) ? string.Empty : (string)x[1],
                Lastname = (x[2] == DBNull.Value) ? string.Empty : (string)x[2],
            });

            return ret.data;
        }


        public static async Task<bl.model.Customer> ExecuteQueryNameAsync(string firstname)
        {
            string sqlwhereName = $@"
                     SELECT
                        Id
                        ,Firstname
                        ,Lastname
                    FROM {bl.refs.Databse_DB}.dbo.pcpms_customer
                    WHERE Firstname = @Firstname";


            var par = new List<Microsoft.Data.SqlClient.SqlParameter>
            {
                new Microsoft.Data.SqlClient.SqlParameter{ ParameterName = "@Firstname", Value = firstname}
            };

            var ret = await bl.DBaccess.RawSqlQuerySingleAsync(sqlwhereName, par, x => new bl.model.Customer
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
    }
}
