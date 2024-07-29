namespace bl.model
{
    public class Report
    {
        public Guid Id { get; set; }
        public Guid UserGenerateReport { get; set; }
        public string NameFileGenerateReport { get; set; }
        public DateTime DateGenerateReport { get; set; }
        public bool PDF { get; set; }
        public bool XMLS { get; set; }
        public bool CSV { get; set; }
        public string filter { get; set; }

        public string Status { get; set; } // Pending / Success
        public string ReportEmty { get; set; }

        public class ReportDataUserCat
        {
            public Guid Id { get; set; }
            public string Username { get; set; }
            public string NameFileGenerateReport { get; set; }
            public DateTime DateGenerateReport { get; set; }
            public bool PDF { get; set; }
            public bool XMLS { get; set; }
            public bool CSV { get; set; }
            public string filter { get; set; }
            public string Status { get; set; } // Pending / Success
            public string ReportEmty { get; set; }
        }

        public static async Task<List<bl.model.Report>> GetPSCAllAsync()
        {
            var ret  = await bl.data.Report.PSCReportExecuteQueryAsync();

            return ret.data;
        }

        public static async Task<List<bl.model.Report.ReportDataUserCat>> GetSuccessPSCAllAsync(string name)
        {
            var ret = await bl.data.Report.ExecuteSuccessQueryNameAsync();

            var query = from rets in ret
                        where rets.NameFileGenerateReport == name
                        select new ReportDataUserCat
                        {

                            Id = rets.Id,
                            Username = rets.Username,
                            NameFileGenerateReport = rets.NameFileGenerateReport,
                            PDF = rets.PDF,
                            XMLS = rets.XMLS,
                            CSV = rets.CSV,
                            Status = rets.Status,
                            filter = rets.filter,
                            ReportEmty = rets.ReportEmty
                        };
    

            return query.ToList();
        }


        public static async Task<List<bl.model.Report.ReportDataUserCat>> GetPendingPSCAllAsync(string name)
        {
            var ret = await bl.data.Report.ExecutePindingQueryNameAsync();

            var query = from rets in ret
                        where rets.NameFileGenerateReport == name
                        select new ReportDataUserCat
                        {

                            Id = rets.Id,
                            Username = rets.Username,
                            NameFileGenerateReport = rets.NameFileGenerateReport,
                            PDF = rets.PDF,
                            XMLS = rets.XMLS,
                            CSV = rets.CSV,
                            Status = rets.Status,
                            filter = rets.filter,
                            ReportEmty = rets.ReportEmty
                        };


            return query.ToList();
        }


        

    }


}
