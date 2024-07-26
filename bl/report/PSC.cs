using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;

namespace bl.report
{
    public class PSC
    {
        public string CatigoriesName { get; set; }
        public string ManufatureName { get; set; }
        public int Stock { get; set; }
        public decimal Price { get; set; }
        public string Specification { get; set; }
        public string Description { get; set; }


        // Method to create and generate a report based on the filter and user information
        public static async Task CreateGenerateReport(string filter, Guid user)
        {
            // Add <br> tag to the filter and remove it later
            var Filter = $"{filter}<br>";
            string cleanedCategoryName = Filter.Replace("<br>", "");

            // Execute query to get product data based on cleaned category name
            var pscList = await bl.data.Manufaturies.RCSExecuteQueryAsync(cleanedCategoryName);

            // Create a new report entry
            var report = new bl.dto.Report
            {
                UserGenerateReport = user,
                NameFileGenerateReport = bl.refs.PSC,
                DateGenerateReport = DateTime.Now,
                PDF = false,
                XMLS = true,
                CSV = false,
                filter = filter == null ? "ALL<br>" : Filter,
                ReportNotEmpty = pscList.Count > 0 ? "Not Empty" : "Empty"
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
        }


        // Method to get Field ID values based on Category Name
        public static async Task<string> GetFieldIDValues(string CategoryName)
        {
            // Retrieve category data
            var data = await bl.data.Categories.ExecuteQueryAsync();

            // Find and return the ID of the specified category
            var FieldIDValues = data.Where(x => x.CategoryName == CategoryName)
                                    .Select(x => x.Id)
                                    .FirstOrDefault()
                                    .ToString();

            return FieldIDValues;
        }

        // Method to get Field Name values based on Category Name
        public static async Task<string> GetFieldNameValues(string categoryName)
        {
            // Retrieve category data
            var data = await bl.data.Categories.ExecuteQueryAsync();

            // Return "ALL" if the category name is "ALL"; otherwise, return the category name
            if (categoryName == "ALL")
            {
                return "ALL";
            }
            else
            {
                var fieldNameValue = data
                    .Where(x => x.CategoryName == categoryName)
                    .Select(x => x.CategoryName)
                    .FirstOrDefault();

                return fieldNameValue;
            }
        }

        // Method to get all sale item IDs based on category name
        public static async Task<List<bl.report.PSC>> GetallSaleItemID(string catname)
        {
            if (catname == "ALL")
            {
                // Retrieve all sale items if category name is "ALL"
                var data = await bl.data.Manufaturies.RCSExecuteQueryAsync();
                return data.ToList();
            }
            else
            {
                // Retrieve sale items for the specified category
                var data = await bl.data.Manufaturies.RCSExecuteQueryAsync(catname);
                return data.ToList();
            }
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
        private static async Task<List<bl.report.PSC>> FormatData(bl.model.Report report)
        {
            // Clean the filter value from the report
            string cleanedFilter = report.filter.Replace("<br>", string.Empty);

            // Get the category name from the cleaned filter
            var reportFilter = await GetFieldNameValues(cleanedFilter);

            // Retrieve all sale items based on the category name
            var pscList = await GetallSaleItemID(reportFilter);

            return pscList;
        }

        // Method to generate XLS file from report data
        private static async Task<Byte[]> generateXLS(List<bl.report.PSC> reportdata, bl.model.Report report)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            // Clean the filter value from the report
            string cleanedFilter = report.filter.Replace("<br>", string.Empty);
            var reportFilter = await GetFieldNameValues(cleanedFilter);

            // Retrieve user details based on the user ID from the report
            var user = await bl.data.User.CheackUserIDHave(report.UserGenerateReport);

            using (var package = new ExcelPackage())
            {
                // Add a new worksheet to the Excel package
                var ws = package.Workbook.Worksheets.Add(bl.refs.PSC);

                SetReportHeader(ws, $"A1", "Report Products Specific By Category");
                SetReportHeader(ws, $"A2", "Category");
                SetReportHeader(ws, $"B2", reportFilter);
                SetReportHeader(ws, $"A3", "Generated User");
                SetReportHeader(ws, $"B3", user.Username);
                SetReportHeader(ws, $"A4", "Generated Date");
                SetReportHeader(ws, $"B4", DateTime.Now.ToString("MM/dd/yyyy"));

                // Merge cells for the column headers
                int colHeader = 5;
                ws.Cells[$"A{colHeader}:A{colHeader + 2}"].Merge = true;
                ws.Cells[$"B{colHeader}:B{colHeader + 2}"].Merge = true;
                ws.Cells[$"C{colHeader}:C{colHeader + 2}"].Merge = true;
                ws.Cells[$"D{colHeader}:D{colHeader + 2}"].Merge = true;
                ws.Cells[$"E{colHeader}:E{colHeader + 2}"].Merge = true;
                ws.Cells[$"F{colHeader}:F{colHeader + 2}"].Merge = true;
                SetBorder(ws.Cells[$"A{colHeader}:F{colHeader + 2}"]);


                SetColumnHeader(ws, $"A{colHeader}", "Categories Name", Color.Aqua);
                SetColumnHeader(ws, $"B{colHeader}", "Manufacturer Name", Color.Aqua);
                SetColumnHeader(ws, $"C{colHeader}", "Stock", Color.Aqua);
                SetColumnHeader(ws, $"D{colHeader}", "Price", Color.Aqua);
                SetColumnHeader(ws, $"E{colHeader}", "Specification", Color.Aqua);
                SetColumnHeader(ws, $"F{colHeader}", "Description", Color.Aqua);

                // Populate worksheet with report data
                int startRow = 8;
                foreach (var row in reportdata)
                {
                    ws.Cells[startRow, 1].Value = row.CatigoriesName;
                    ws.Cells[startRow, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    ws.Cells[startRow, 2].Value = row.ManufatureName;
                    ws.Cells[startRow, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    ws.Cells[startRow, 3].Value = row.Stock;
                    ws.Cells[startRow, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    ws.Cells[startRow, 4].Value = row.Price == null ? "" : ((decimal)row.Price).ToString("N2");
                    ws.Cells[startRow, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    ws.Cells[startRow, 5].Value = row.Specification;
                    ws.Cells[startRow, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[startRow, 6].Value = row.Description;
                    ws.Cells[startRow, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    startRow++;
                }

                // Set column widths
                ws.Column(1).Width = 35;
                ws.Column(2).Width = 23;
                ws.Column(3).Width = 15;
                ws.Column(4).Width = 15;
                ws.Column(5).Width = 60;
                ws.Column(6).Width = 23;

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
