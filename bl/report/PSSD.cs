using OfficeOpenXml.Style;
using OfficeOpenXml;
using System.Drawing;
using Microsoft.VisualBasic;

namespace bl.report
{
    public class PSSD
    {

        public string ManufatureName { get; set; }
        public int TotalQuantitySold { get; set; }
        public decimal TotalSalesAmount { get; set; }




        public static async Task<string> CreateGenerateReport(DateTime startDate, DateTime endDate, Guid user)
        {
            if (startDate > endDate) return "The start date cannot be later than the end date.";
            if (startDate == endDate) return "The start date cannot be the same as the end date.";
            if (endDate > DateTime.Now) return "The end date cannot be in the future.";

            var pssdList = await bl.data.Sales.PSSDExecuteQueryAsync(startDate, endDate);

            // Initialize the report
            var report = new bl.dto.Report
            {
                UserGenerateReport = user,
                NameFileGenerateReport = bl.refs.PSSD,
                DateGenerateReport = DateTime.Now,
                PDF = false,
                XMLS = true,
                CSV = false,
                filter = $"{startDate:yyyy-MM-dd}<br>{endDate:yyyy-MM-dd}", // Ensure the date format is consistent
                ReportNotEmpty = pssdList.Count > 0 ? "Not Empty" : "Empty"
            };


            // Insert the report entry into the database
            await bl.data.Report.InsertDataAsync(report);

            // Start the report update task
            _ = HandleReportUpdateAsync();

            // Method to handle report update after a delay
            async Task HandleReportUpdateAsync()
            {

                await Task.Delay(35000);  // Delay to simulate processing time

                // Retrieve pending reports
                var pendingReports = await bl.data.Report.ExecutePindingQueryNameAsync();

                if (pendingReports.Any())
                {
                    var reportToUpdate = pendingReports.First();
                    // Update the report status
                    await bl.data.Report.UpdateDataAsync(reportToUpdate.Id);
                }
            }

            return "";
        }




        // Method to get all sale item IDs based on category name
        public static async Task<List<bl.report.PSSD>> GetallSaleProductDate(DateTime startDate, DateTime endDate)
        {
            var data = await bl.data.Sales.PSSDExecuteQueryAsync(startDate, endDate);

            return data;
        }


        public static string[] RemoveFilterBrFromArray(string[] reportFilters)
        {
            // Create a new array to hold the cleaned strings
            string[] cleanedFilters = new string[reportFilters.Length];

            // Iterate over each string in the input array
            for (int i = 0; i < reportFilters.Length; i++)
            {
                cleanedFilters[i] = reportFilters[i].Replace("<br>", "|"); // Use a delimiter to separate dates
            }

            // Split the single string into multiple parts
            return cleanedFilters[0].Split('|');
        }


        // Method to create a report for download based on the report ID
        public static async Task<Byte[]> CraeteToDownlodReport(Guid id)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            // Retrieve report details based on the provided ID
            var item = await bl.data.Report.ExecuteQueryIDAsync(id);

            // Format data for the report
            var reportData = await FormatData(item);

            // Generate XLS file and return its byte array
            byte[] fileContents = await generateXLS(reportData, item);

            return fileContents;
        }
        // Method to format data for the report
        private static async Task<List<bl.report.PSSD>> FormatData(bl.model.Report report)
        {
            var filtersArray = new string[] { report.filter };


            // Remove <br> tags from the array of filters
            var cleanedFilterArray = RemoveFilterBrFromArray(filtersArray);

            DateTime startDate;
            DateTime endDate;

            // Parse the date strings with error handling
            DateTime.TryParse(cleanedFilterArray[0], out startDate);
            DateTime.TryParse(cleanedFilterArray[1], out endDate);
   

            var pssdList = await GetallSaleProductDate(startDate, endDate);

            return pssdList;



        }


        private static async Task<Byte[]> generateXLS(List<bl.report.PSSD> reportdata, bl.model.Report report)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var filtersArray = new string[] { report.filter };


            // Remove <br> tags from the array of filters
            var cleanedFilterArray = RemoveFilterBrFromArray(filtersArray);

            // Since cleanedFilterArray contains one element, get that element
            DateTime startDate = Convert.ToDateTime(cleanedFilterArray[0]);
            DateTime endDate = Convert.ToDateTime(cleanedFilterArray[1]);

      

            // Retrieve user details based on the user ID from the report
            var user = await bl.data.User.CheackUserIDHave(report.UserGenerateReport);


            using (var package = new ExcelPackage())
            {
                // Add a new worksheet to the Excel package
                var ws = package.Workbook.Worksheets.Add(bl.refs.PSSD);

                SetReportHeader(ws, $"A1", "Report Sold Specific Date Range");

                SetReportHeader(ws, $"A2", "DateTime");
                SetReportHeader(ws, "B2", $"{startDate:yyyy-MM-dd} TO {endDate:yyyy-MM-dd}");
                SetReportHeader(ws, $"A3", "Generated User");
                SetReportHeader(ws, $"B3", user.Username);
                SetReportHeader(ws, $"A4", "Generated Date");
                SetReportHeader(ws, $"B4", DateTime.Now.ToString("MM/dd/yyyy"));

                // Merge cells for the column headers
                int colHeader = 5;
                ws.Cells[$"A{colHeader}:A{colHeader + 2}"].Merge = true;
                ws.Cells[$"B{colHeader}:B{colHeader + 2}"].Merge = true;
                ws.Cells[$"C{colHeader}:C{colHeader + 2}"].Merge = true;
                SetBorder(ws.Cells[$"A{colHeader}:C{colHeader + 2}"]);


                SetColumnHeader(ws, $"A{colHeader}", "ProductName", Color.Aqua);
                SetColumnHeader(ws, $"B{colHeader}", "Total QTY Sold", Color.Aqua);
                SetColumnHeader(ws, $"C{colHeader}", "Total Sales Amount", Color.Aqua);

                // Populate worksheet with report data
                int startRow = 8;
                foreach (var row in reportdata)
                {
                    ws.Cells[startRow, 1].Value = row.ManufatureName;
                    ws.Cells[startRow, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    ws.Cells[startRow, 2].Value = row.TotalQuantitySold;
                    ws.Cells[startRow, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    ws.Cells[startRow, 3].Value = row.TotalSalesAmount == null ? "" : ((decimal)row.TotalSalesAmount).ToString("N2");
                    ws.Cells[startRow, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
     
                    startRow++;
                }

                // Set column widths
                ws.Column(1).Width = 35;
                ws.Column(2).Width = 21;
                ws.Column(3).Width = 21;


                // Save and return the Excel file as a byte array
                byte[] xls = package.GetAsByteArray();
                return xls;
            }
        }


        // Method to set column header style
        private static void SetColumnHeader(ExcelWorksheet ws, string rowcol, string headerText, Color bgcolor)
        {
            ws.Cells[rowcol].IsRichText = true;
            ws.Cells[rowcol].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells[rowcol].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            ws.Cells[rowcol].Style.WrapText = true;

            // Set the background color
            ws.Cells[rowcol].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells[rowcol].Style.Fill.BackgroundColor.SetColor(bgcolor);

            var style = ws.Cells[rowcol].RichText.Add(headerText);
            style.Bold = true;
        }

        // Method to set report header style
        private static void SetReportHeader(ExcelWorksheet ws, string rowcol, string headerText)
        {
            ws.Cells[rowcol].IsRichText = true;
            var style = ws.Cells[rowcol].RichText.Add(headerText);
            style.Bold = true;
        }

        // Method to set border style for cells
        private static void SetBorder(ExcelRange cell)
        {
            cell.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            cell.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            cell.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            cell.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
        }
    }
}
