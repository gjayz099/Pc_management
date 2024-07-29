using OfficeOpenXml.Style;
using OfficeOpenXml;
using System.Drawing;

namespace bl.report
{
    public class PSCSB
    {
        public string CategoryName { get; set; }
        public string ManufacturerName { get; set; }
        public int Stock {  get; set; }

        public static async Task<string> CreateGenerateReport(Guid CategoryID, string Stock, Guid user)
        {
      
            if (!int.TryParse(Stock, out int stock)) return "Stock Is Zero";

            var pscsbList = await bl.data.Manufaturies.RPSCSBExecuteQueryAsync(CategoryID, stock);

            // Initialize the report
            // Initialize the report
            var report = new bl.dto.Report
            {
                UserGenerateReport = user,
                NameFileGenerateReport = bl.refs.PSCSB,
                DateGenerateReport = DateTime.Now,
                PDF = false,
                XMLS = true,
                CSV = false,
                ReportNotEmpty = pscsbList.Count > 0 ? "Not Empty" : "Empty"
            };

            // Check if the CategoryID is valid
            if (CategoryID != Guid.Empty)
            {
                var catId = await bl.data.Categories.CheckCategoriesId(CategoryID);

                report.filter = $"{catId.CategoryName}<br>{Stock}";

            }
            else
            {
                // Handle the case where CategoryID is not found
                report.filter = $"ALL<br>{Stock}";
            }

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
        public static async Task<string> GetFieldNameValues(string CategoryName)
        {
            // Retrieve category data
            var data = await bl.data.Categories.ExecuteQueryAsync();

            var fieldNameValue = data.Where(x => x.CategoryName == CategoryName)
                                    .Select(x => x.CategoryName)
                                    .FirstOrDefault();
            return fieldNameValue ?? "ALL";
        }


        // Method to get all sale item IDs based on category name
        public static async Task<List<bl.report.PSCSB>> GetallCatWithStockProductDate(Guid categoryName, int stock)
        {
            var data = await bl.data.Manufaturies.RPSCSBExecuteQueryAsync(categoryName, stock);

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


        private static async Task<List<bl.report.PSCSB>> FormatData(bl.model.Report report)
        {
            var filtersArray = new string[] { report.filter };


            // Remove <br> tags from the array of filters
            var cleanedFilterArray = RemoveFilterBrFromArray(filtersArray);

            var category = cleanedFilterArray[0];
            var stockString = cleanedFilterArray[1];

            // Retrieve the category GUID from the filter
            var reportFilter = await GetFieldIDValues(category);

            Guid.TryParse(reportFilter, out var categoryGuid);
            int.TryParse(stockString, out int stock);



            var pssdList = await GetallCatWithStockProductDate(categoryGuid, stock);

            return pssdList;

        }

        private static async Task<Byte[]> generateXLS(List<bl.report.PSCSB> reportdata, bl.model.Report report)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var filtersArray = new string[] { report.filter };

            // Remove <br> tags from the array of filters
            var cleanedFilterArray = RemoveFilterBrFromArray(filtersArray);

            // Since cleanedFilterArray contains one element, get that element
            var Category = cleanedFilterArray[0];
            var Stock = cleanedFilterArray[1];

            var reportFilter = await GetFieldNameValues(Category);

            // Retrieve user details based on the user ID from the report
            var user = await bl.data.User.CheackUserIDHave(report.UserGenerateReport);


            using (var package = new ExcelPackage())
            {
                // Add a new worksheet to the Excel package
                var ws = package.Workbook.Worksheets.Add(bl.refs.PSC);

                SetReportHeader(ws, $"A1", "Products Category With Stock Below");
                SetReportHeader(ws, $"A2", "Category");
                SetReportHeader(ws, $"B2", reportFilter+ " & " + Stock);
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


                SetColumnHeader(ws, $"A{colHeader}", "Categories Name", Color.Aqua);
                SetColumnHeader(ws, $"B{colHeader}", "Manufacturer Name", Color.Aqua);
                SetColumnHeader(ws, $"C{colHeader}", "Stock", Color.Aqua);

                // Populate worksheet with report data
                int startRow = 8;
                foreach (var row in reportdata)
                {
                    ws.Cells[startRow, 1].Value = row.CategoryName;
                    ws.Cells[startRow, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    ws.Cells[startRow, 2].Value = row.ManufacturerName;
                    ws.Cells[startRow, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    ws.Cells[startRow, 3].Value = row.Stock;
                    startRow++;
                }

                // Set column widths
                ws.Column(1).Width = 35;
                ws.Column(2).Width = 18;
                ws.Column(3).Width = 18;
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
