﻿using OfficeOpenXml;
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


        public static async Task<string> CreateGenerateReport(Guid CategoryID, Guid user)
        {
            var pscList = await bl.data.Manufaturies.RCSExecuteQueryAsync(CategoryID);

            // Initialize the report
            var report = new bl.dto.Report
            {
                UserGenerateReport = user,
                NameFileGenerateReport = bl.refs.PSC,
                DateGenerateReport = DateTime.Now,
                PDF = false,
                XMLS = true,
                CSV = false,
                ReportNotEmpty = pscList.Count > 0 ? "Not Empty" : "Empty"
            };

            // Check if the CategoryID is valid
            if (CategoryID != Guid.Empty)
            {
                var catId = await bl.data.Categories.CheckCategoriesId(CategoryID);

                report.filter = $"{catId.CategoryName}<br>";

            }
            else
            {
                // Handle the case where CategoryID is not found
                report.filter = "ALL<br>";
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
            return fieldNameValue?? "ALL";
        }

        // Method to get all sale item IDs based on category name
        public static async Task<List<bl.report.PSC>> GetallSaleItemID(Guid categoryName)
        {
            var data = await bl.data.Manufaturies.RCSExecuteQueryAsync(categoryName);

            return data;
        }

        public static string[] RemoveFilterBrFromArray(string[] reportFilters)
        {
            // Create a new array to hold the cleaned strings
            string[] cleanedFilters = new string[reportFilters.Length];

            // Iterate over each string in the input array
            for (int i = 0; i < reportFilters.Length; i++)
            {
                // Remove all occurrences of <br> tags from the current string
                cleanedFilters[i] = reportFilters[i].Replace("<br>", "|"); // Use a delimiter to separate dates
            }

            return cleanedFilters;
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
            var filtersArray = new string[] { report.filter };


            // Remove <br> tags from the array of filters
            var cleanedFilterArray = RemoveFilterBrFromArray(filtersArray);

            // Since cleanedFilterArray contains one element, get that element
            var Category = cleanedFilterArray[0];


            // Get the category name from the cleaned filter
            var reportFilter = await GetFieldIDValues(Category);

            // Try to convert reportFilter to Guid if needed
            if (!Guid.TryParse(reportFilter, out var categoryGuid))
            {
                throw new ArgumentException("The report filter value is not a valid GUID.");
            }

            // Retrieve all sale items based on the category GUID
            var pscList = await GetallSaleItemID(categoryGuid);

            return pscList;
        }

        // Method to generate XLS file from report data
        private static async Task<Byte[]> generateXLS(List<bl.report.PSC> reportdata, bl.model.Report report)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var filtersArray = new string[] { report.filter };

            // Remove <br> tags from the array of filters
            var cleanedFilterArray = RemoveFilterBrFromArray(filtersArray);

            // Since cleanedFilterArray contains one element, get that element
            var Category = cleanedFilterArray[0];

            var reportFilter = await GetFieldNameValues(Category);

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
