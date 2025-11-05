using OfficeOpenXml;

namespace CSharpSeleniumNUnitAutomationProject.Utilities
{
    public static class ExcelReader
    {
        public static List<Dictionary<string, string>> ReadExcel(string filePath, string sheetName)
        {
            var result = new List<Dictionary<string, string>>();

            FileInfo fileInfo = new FileInfo(filePath);
            ExcelPackage.License.SetNonCommercialPersonal("Test");

            using (var package = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[sheetName];
                int colCount = worksheet.Dimension.End.Column;
                int rowCount = worksheet.Dimension.End.Row;

                // Get column headers
                var headers = new List<string>();
                for (int col = 1; col <= colCount; col++)
                {
                    headers.Add(worksheet.Cells[1, col].Text);
                }

                // Read each row
                for (int row = 2; row <= rowCount; row++)
                {
                    var rowData = new Dictionary<string, string>();
                    for (int col = 1; col <= colCount; col++)
                    {
                        rowData[headers[col - 1]] = worksheet.Cells[row, col].Text;
                    }
                    result.Add(rowData);
                }
            }

            return result;
        }
    }
}
