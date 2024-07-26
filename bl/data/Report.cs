namespace bl.data
{
    public  class Report
    {

        private static string sqlAll = $@"
                            SELECT Id
                                ,UserGenerateReport
		                        ,NameFileGenerateReport
		                        ,DateGenerateReport
		                        ,PDF
		                        ,XMLS
		                        ,CSV
                                ,Filter
                                ,Status
                                ,ReportNotEmpty
                        FROM {bl.refs.Databse_DB}.dbo.pcpms_report";


        public static async Task<(bool err, string stauts, List<bl.model.Report> data)> PSCReportExecuteQueryAsync()
        {

            var ret = await bl.DBaccess.OldRawSqlQueryAsync(sqlAll, x => new bl.model.Report
            {
                Id = (x[0] == DBNull.Value) ? Guid.Empty : (Guid)x[0],
                UserGenerateReport = (x[1] == DBNull.Value) ? Guid.Empty : (Guid)x[1],
                NameFileGenerateReport = (x[2] == DBNull.Value) ? string.Empty : (string)x[2],
                DateGenerateReport = (x[3] == DBNull.Value) ? DateTime.MinValue : (DateTime)x[3],
                PDF = (x[4] == DBNull.Value) ? false : Convert.ToBoolean(x[4]),
                XMLS = (x[5] == DBNull.Value) ? false : Convert.ToBoolean(x[5]),
                CSV = (x[6] == DBNull.Value) ? false : Convert.ToBoolean(x[6]),
                filter = (x[7] == DBNull.Value) ? string.Empty : (string)x[7],
                Status = (x[8] == DBNull.Value) ? string.Empty : (string)x[8],
                ReportEmty = (x[9] == DBNull.Value) ? string.Empty : (string)x[9],
            });

            return (false, "Success", ret);
        }


        public static async Task<List<bl.model.Report.ReportDataUserCat>> ExecutePindingQueryNameAsync()
        {

           string sqlwhereID = $@"
                    SELECT rt.Id
                         ,NameFileGenerateReport
		                ,(SELECT Username FROM {bl.refs.Databse_DB}.dbo.pcpms_user sr WHERE sr.Id = rt.UserGenerateReport)
		                ,rt.DateGenerateReport
		                ,rt.PDF
		                ,rt.XMLS
		                ,rt.CSV
                        ,rt.Filter
                        ,rt.Status
                        ,rt.ReportNotEmpty
                        FROM {bl.refs.Databse_DB}.dbo.pcpms_report rt
                        WHERE rt.NameFileGenerateReport = @NameFileGenerateReport AND rt.Status = @Status";

            var par = new List<Microsoft.Data.SqlClient.SqlParameter>
            {
                new Microsoft.Data.SqlClient.SqlParameter{ ParameterName = "@NameFileGenerateReport", Value = bl.refs.PSC},
                new Microsoft.Data.SqlClient.SqlParameter{ ParameterName = "@Status", Value = bl.refs.Report_Status_Pending}
            };

            var ret = await bl.DBaccess.RawSqlQueryAsync(sqlwhereID, par, x => new bl.model.Report.ReportDataUserCat
            {
                Id = (x[0] == DBNull.Value) ? Guid.Empty : (Guid)x[0],
                NameFileGenerateReport = (x[1] == DBNull.Value) ? string.Empty : (string)x[1],
                Username = (x[2] == DBNull.Value) ? string.Empty : (string)x[2],
                DateGenerateReport = (x[3] == DBNull.Value) ? DateTime.MinValue : (DateTime)x[3],
                PDF = (x[4] == DBNull.Value) ? false : Convert.ToBoolean(x[4]),
                XMLS = (x[5] == DBNull.Value) ? false : Convert.ToBoolean(x[5]),
                CSV = (x[6] == DBNull.Value) ? false : Convert.ToBoolean(x[6]),
                filter = (x[7] == DBNull.Value) ? string.Empty : (string)x[7],
                Status = (x[8] == DBNull.Value) ? string.Empty : (string)x[8],
                ReportEmty = (x[9] == DBNull.Value) ? string.Empty : (string)x[9],
            });

            return ret.data;
        }

        public static async Task<List<bl.model.Report.ReportDataUserCat>> ExecuteSuccessQueryNameAsync()
        {

            string sqlwhereID = $@"
                    SELECT rt.Id
                         ,NameFileGenerateReport
		                ,(SELECT Username FROM {bl.refs.Databse_DB}.dbo.pcpms_user sr WHERE sr.Id = rt.UserGenerateReport)
		                ,rt.DateGenerateReport
		                ,rt.PDF
		                ,rt.XMLS
		                ,rt.CSV
                        ,rt.Filter
                        ,rt.Status
                        ,rt.ReportNotEmpty
                        FROM {bl.refs.Databse_DB}.dbo.pcpms_report rt
                        WHERE rt.NameFileGenerateReport = @NameFileGenerateReport AND rt.Status = @Status";

            var par = new List<Microsoft.Data.SqlClient.SqlParameter>
            {
                new Microsoft.Data.SqlClient.SqlParameter{ ParameterName = "@NameFileGenerateReport", Value = bl.refs.PSC},
                new Microsoft.Data.SqlClient.SqlParameter{ ParameterName = "@Status", Value = bl.refs.Report_Status_Succes }
            };

            var ret = await bl.DBaccess.RawSqlQueryAsync(sqlwhereID, par, x => new bl.model.Report.ReportDataUserCat
            {
                Id = (x[0] == DBNull.Value) ? Guid.Empty : (Guid)x[0],
                NameFileGenerateReport = (x[1] == DBNull.Value) ? string.Empty : (string)x[1],
                Username = (x[2] == DBNull.Value) ? string.Empty : (string)x[2],
                DateGenerateReport = (x[3] == DBNull.Value) ? DateTime.MinValue : (DateTime)x[3],
                PDF = (x[4] == DBNull.Value) ? false : Convert.ToBoolean(x[4]),
                XMLS = (x[5] == DBNull.Value) ? false : Convert.ToBoolean(x[5]),
                CSV = (x[6] == DBNull.Value) ? false : Convert.ToBoolean(x[6]),
                filter = (x[7] == DBNull.Value) ? string.Empty : (string)x[7],
                Status = (x[8] == DBNull.Value) ? string.Empty : (string)x[8],
                ReportEmty = (x[9] == DBNull.Value) ? string.Empty : (string)x[9],
            });

            return ret.data;
        }

        public static async Task<int> InsertDataAsync(bl.dto.Report dto)
        {
            string sql = $@"INSERT INTO {bl.refs.Databse_DB}.dbo.pcpms_report
                            (    
                            Id
                            ,UserGenerateReport
		                    ,NameFileGenerateReport
		                    ,DateGenerateReport
		                    ,PDF
		                    ,XMLS
		                    ,CSV
                            ,Filter
                            ,Status
                            ,ReportNotEmpty
                            )
                    VALUES
                            (
                             NEWID()
                            ,@UserGenerateReport
		                    ,@NameFileGenerateReport
		                    ,@DateGenerateReport
		                    ,@PDF
		                    ,@XMLS
		                    ,@CSV
                            ,@Filter
                            ,@Status 
                            ,@ReportNotEmpty     
                            )";

            var ret = await bl.DBaccess.OldExecNonQueryAsync(sql, new List<Microsoft.Data.SqlClient.SqlParameter>
            {
                new Microsoft.Data.SqlClient.SqlParameter{ ParameterName = "@UserGenerateReport", Value = dto.UserGenerateReport },
                new Microsoft.Data.SqlClient.SqlParameter{ ParameterName = "@NameFileGenerateReport", Value = dto.NameFileGenerateReport },
                new Microsoft.Data.SqlClient.SqlParameter{ ParameterName = "@DateGenerateReport", Value = DateTime.Now},
                new Microsoft.Data.SqlClient.SqlParameter{ ParameterName = "@PDF", SqlDbType = System.Data.SqlDbType.Bit, Value = (object)dto.PDF ?? false},
                new Microsoft.Data.SqlClient.SqlParameter{ ParameterName = "@XMLS", SqlDbType = System.Data.SqlDbType.Bit, Value = (object)dto.XMLS ?? false},
                new Microsoft.Data.SqlClient.SqlParameter{ ParameterName = "@CSV", SqlDbType = System.Data.SqlDbType.Bit, Value = (object)dto.CSV ?? false},
                new Microsoft.Data.SqlClient.SqlParameter{ ParameterName = "@Filter", Value = dto.filter},
                new Microsoft.Data.SqlClient.SqlParameter{ ParameterName = "@Status", Value = refs.Report_Status_Pending },
                new Microsoft.Data.SqlClient.SqlParameter{ ParameterName = "@ReportNotEmpty", Value = dto.ReportNotEmpty},

            });

            return ret;
        }


        public static async Task<int> UpdateDataAsync(Guid id)
        {
            string sql = $@"UPDATE {bl.refs.Databse_DB}.dbo.pcpms_report SET   
                            Status = @Status where Id = '{id}'";

            var ret = await bl.DBaccess.OldExecNonQueryAsync(sql, new List<Microsoft.Data.SqlClient.SqlParameter>
            {
                new Microsoft.Data.SqlClient.SqlParameter{ ParameterName = "@Status", Value = refs.Report_Status_Succes },
            });

            return ret;
        }



        public static async Task<bl.model.Report> ExecuteQueryIDAsync(Guid guid)
        {

            string sqlwhereID = $@"{sqlAll} WHERE Id = @Id";


            var par = new List<Microsoft.Data.SqlClient.SqlParameter>
            {
                new Microsoft.Data.SqlClient.SqlParameter{ ParameterName = "@Id", Value = guid}
            };

            var ret = await bl.DBaccess.RawSqlQuerySingleAsync(sqlwhereID, par, x => new bl.model.Report
            {
                Id = (x[0] == DBNull.Value) ? Guid.Empty : (Guid)x[0],
                UserGenerateReport = (x[1] == DBNull.Value) ? Guid.Empty : (Guid)x[1],
                NameFileGenerateReport = (x[2] == DBNull.Value) ? string.Empty : (string)x[2],
                DateGenerateReport = (x[3] == DBNull.Value) ? DateTime.MinValue : (DateTime)x[3],
                PDF = (x[4] == DBNull.Value) ? false : Convert.ToBoolean(x[4]),
                XMLS = (x[5] == DBNull.Value) ? false : Convert.ToBoolean(x[5]),
                CSV = (x[6] == DBNull.Value) ? false : Convert.ToBoolean(x[6]),
                filter = (x[7] == DBNull.Value) ? string.Empty : (string)x[7],
            });

            return ret.data;
        }
    }


}
