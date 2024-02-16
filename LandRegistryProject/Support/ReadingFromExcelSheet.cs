using BoDi;
using LandRegistryProject.Drivers;
using LandRegistryProject.PageObject;
using Microsoft.Office.Interop.Excel;
using NUnit.Framework;
using OfficeOpenXml;
using OpenQA.Selenium;
using System.Text;

namespace LandRegistryProject.Support
{
    public class ReadingFromExcelSheet : DriverHelper
    {

        public ExcelWorksheet worksheet;
        public ExcelPackage pck;
        public ExcelWorksheet sheet;
        public LoginPage loginPage;
        public DirectoryInfo projDir = new DirectoryInfo(Environment.CurrentDirectory);
        public string excelFilePath;
        
        

        public ReadingFromExcelSheet(IObjectContainer container)
        {
            driver = container.Resolve<IWebDriver>();
            loginPage = container.Resolve<LoginPage>();

        }
            public void ReadExcelData()
        {
            excelFilePath = projDir.Parent.Parent.Parent.FullName + @"\TestDatas\TittleNumer.xlsx";
            var newFile = new FileInfo(excelFilePath);
            pck = new OfficeOpenXml.ExcelPackage(newFile);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            sheet = pck.Workbook.Worksheets[0];

            var sb = new StringBuilder();
            int rows = sheet.Dimension.Rows;
            int cols = sheet.Dimension.Columns;

            for (int r = 3; r <= rows; r++)
            {
                
                if (sheet.Cells[r, 13].Text.Equals(""))
                {
                    string rg = "A" + r + ":M" + r;
                   // List<string> rowdata = ReadRowData(rg);
                    Dictionary<string, string> RData = ReadRowData(r);
                    Othersteps(RData, r);
                }
            }
        }



        public Dictionary<string, string> ReadRowData(int row)
        {
            Dictionary<string, string> RowData = new Dictionary<string, string>();

            List<string> cellValue = new List<string>();
            string cellRange = "A" + row + ":M" + row;

            sheet = pck.Workbook.Worksheets[0];


            for (int i = 1; i <= 13; i++)
                RowData.Add(sheet.Cells[3, i].Text.ToString(), sheet.Cells[row, i].Text.ToString());
        
            return RowData;
            
        }

        public List<string> ReadRowData(string cellRange)
        {

            //string excelFilePath = "C:\\Users\\odunayo.olufemi\\OneDrive - Homes England\\Documents\\DSTittleNumber\\TittleNumer.xlsx";
            ////string excelFilePath = Path.Combine(Environment.CurrentDirectory, @"TestDatas\", "TittleNumber.xlsx");
            //ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            //// Load the Excel package from the file
            //FileInfo fileInfo = new FileInfo(excelFilePath);
            List<string> cellValue = new List<string>();
            //using (ExcelPackage package = new ExcelPackage(fileInfo))
            //{
            // Access the worksheet (Feb 2024 in this case)
            sheet = pck.Workbook.Worksheets[0];

            // Read data from Cells
            sheet.Cells[cellRange].ToList().ForEach(x => cellValue.Add(x.Value.ToString()!));
            //}
            return cellValue;
        }



        public int GetColumnByName(string colName)
        {
            int i = 1;
            for (i = 1; i <= 13; i++)
            {
                if (sheet.Cells[3, i].Text.Equals(colName))
                    break;
            }
            return i;
        }


        public void WriteDataToExcelSpreadSheet(int row, int col, string value)
        {
            // string filePath = "C:\\Users\\odunayo.olufemi\\OneDrive - Homes England\\Documents\\DSTittleNumber\\TittleNumer.xlsx";
            excelFilePath = projDir.Parent.Parent.Parent.FullName + @"\TestDatas\TittleNumer.xlsx";
            var ValueToUpdate = value.Length > 20 ? GetSubString(value) : value;
            //var extractedAddress = value.Contains("Wallace") ? GetSubString(value) : value;

                //ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(sheetName);
                //worksheet = package.Workbook.Worksheets[sheetName];
                sheet = pck.Workbook.Worksheets[0];
                // Example data
                //worksheet.Cells[1, 1].Value = "Address";
                //worksheet.Cells[1, 2].Value = "Application Reference";

                sheet.Cells[row, col].Value = ValueToUpdate;

                // Save the file
                pck.SaveAs(new FileInfo(excelFilePath));
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

        public void Othersteps(Dictionary<string, string> rowdata, int row)
        {
            if (!rowdata.GetValueOrDefault("HMLR Title No.").Equals("") && !rowdata.GetValueOrDefault("Full Asset Address").Equals(""))
            {
                Assert.That(!rowdata.GetValueOrDefault("HMLR Title No.").Equals(""), "\n\nERROR: No value in Field 'HMLR Title No'\n\n");

                loginPage.EnterTittleNumberFromExcel(rowdata.GetValueOrDefault("HMLR Title No."));

                loginPage.ClickNextButton();
                var actualAddress = loginPage.GetActualAddress();

                Assert.That(actualAddress.Replace("(", "").Replace(")", "").Contains(rowdata.GetValueOrDefault("Full Asset Address").Replace("(", "").Replace(")", "")), Is.EqualTo(true));
                Assert.That(!rowdata.GetValueOrDefault("Full Asset Address").Equals(""), "\n\nERROR: No value in Field 'Full Asset Address'\n\n");
                loginPage.ClickNextButton();
                loginPage.EnterDateOfCharge();
                loginPage.SelectYesRadioButton();
                var YesMessagesOption = loginPage.IsYesMessagesOptionTicked();
                Assert.That(YesMessagesOption, Is.EqualTo(YesMessagesOption));
                loginPage.ClickNextButton();
                loginPage.ClickNextButton();
                loginPage.EnterCustomerReference();
                loginPage.ClickNextButton();
                var address = loginPage.GetDisplayedAddressDetails();
                WriteDataToExcelSpreadSheet(row, GetColumnByName("HMLR Asset Address"), address);
                var appRef = loginPage.GetDisplayedApplicationReference();
                WriteDataToExcelSpreadSheet(row, GetColumnByName("Discharge Reference"), appRef);
                WriteDataToExcelSpreadSheet(row, GetColumnByName("eDS1 Status"), "Submitted");
                loginPage.ClickeDs1Discharge();
            }
            else
                Console.WriteLine("\n\nRECORD SKIPPED for row: "+ row + "\nNo value in Field 'HMLR Title No' or 'Full Asset Address'.\n\n");
        }
    }
}

