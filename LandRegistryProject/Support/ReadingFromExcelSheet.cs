using FluentAssertions;
using Microsoft.Office.Interop.Excel;
using NUnit.Framework;
using OfficeOpenXml;
using SpecFlow.Internal.Json;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography.X509Certificates;
using Excel = Microsoft.Office.Interop.Excel;

namespace LandRegistryProject.Support
{
    public class ReadingFromExcelSheet
    {
        public void ReadExcelData()
        {
            string filePath = "C:\\Users\\odunayo.olufemi\\OneDrive - Homes England\\Documents\\DSTittleNumber\\TittleNumer.xlsx";
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.First(); // Assuming you're working with the first sheet

                var rowCount = worksheet.Dimension.Rows;
                var colCount = worksheet.Dimension.Columns;


                for (int row = 1; row <= rowCount; row++)
                {
                    for (int col = 1; col < colCount; col++)
                    {
                        Console.Write(worksheet.Cells[row, col].Value + "\t");
                        var text = worksheet.Cells[row, col].Value;
                    }
                    Console.WriteLine();
                }
            }
        }

        public List<string> ReadData(string sheetName, string cellRange)
        {
            string excelFilePath = "C:\\Users\\odunayo.olufemi\\OneDrive - Homes England\\Documents\\DSTittleNumber\\TittleNumer.xlsx";
            //string excelFilePath = Path.Combine(Environment.CurrentDirectory, @"TestDatas\", "TittleNumber.xlsx");
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            // Load the Excel package from the file
            FileInfo fileInfo = new FileInfo(excelFilePath);
            List<string> cellValue = new List<string>();
            using (ExcelPackage package = new ExcelPackage(fileInfo))
            {
                // Access the worksheet (Sheet1 in this case)
                ExcelWorksheet worksheet = package.Workbook.Worksheets[sheetName];

                // Read data from Cells
                worksheet.Cells[cellRange].ToList().ForEach(x => cellValue.Add(x.Value.ToString()!));
                // var cellB1Value = worksheet.Cells["I1:I5"].ToList();
            }
            return cellValue;
        }

        public void WriteDataToExcelSpreadSheet(string sheetName, string cellRange, string value)
        {
            string filePath = "C:\\Users\\odunayo.olufemi\\OneDrive - Homes England\\Documents\\DSTittleNumber\\TittleNumer.xlsx";

            var extractedAddress = value.Length > 20 ? GetSubString(value) : value;
            //var extractedAddress = value.Contains("Wallace") ? GetSubString(value) : value;
            using (var package = new ExcelPackage(filePath))
            {
                //ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(sheetName);
                ExcelWorksheet worksheet = package.Workbook.Worksheets[sheetName];

                // Example data
                //worksheet.Cells[1, 1].Value = "Address";
                //worksheet.Cells[1, 2].Value = "Application Reference";

                worksheet.Cells[cellRange].Value = extractedAddress;

                // Save the file
                package.SaveAs(new FileInfo(filePath));
            }
        }

        public string GetSubString(string value)
        {
            string fullAddress = value;
            string remainingPart = null;

            // Find the index of the first comma
            int indexOfComma = fullAddress.IndexOf(',');

            if (indexOfComma != -1)
            {
                // Extract the substring after the first comma
                remainingPart = fullAddress.Substring(indexOfComma + 1).Trim();

                // Output the separated parts
                Console.WriteLine("First Part: " + fullAddress.Substring(0, indexOfComma).Trim());
                Console.WriteLine("Remaining Part: " + remainingPart);
            }
            else
            {
                // Handle the case where there is no comma in the string
                Console.WriteLine("No comma found in the string.");
            }

            return remainingPart;
        }
    }
}

