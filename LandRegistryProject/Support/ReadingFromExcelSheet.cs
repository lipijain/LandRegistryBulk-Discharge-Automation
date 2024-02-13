using BoDi;
using LandRegistryProject.Drivers;
using LandRegistryProject.PageObject;
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
                    List<string>  rowdata = ReadRowData(rg);
                    Othersteps(rowdata, r);
                }
            }
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



        public void WriteDataToExcelSpreadSheet(string sheetName, string cellRange, string value)
        {
            // string filePath = "C:\\Users\\odunayo.olufemi\\OneDrive - Homes England\\Documents\\DSTittleNumber\\TittleNumer.xlsx";
            excelFilePath = projDir.Parent.Parent.Parent.FullName + @"\TestDatas\TittleNumer.xlsx";
            var extractedAddress = value.Length > 20 ? GetSubString(value) : value;
            //var extractedAddress = value.Contains("Wallace") ? GetSubString(value) : value;

                //ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(sheetName);
                //worksheet = package.Workbook.Worksheets[sheetName];
                sheet = pck.Workbook.Worksheets[0];
                // Example data
                //worksheet.Cells[1, 1].Value = "Address";
                //worksheet.Cells[1, 2].Value = "Application Reference";

                sheet.Cells[cellRange].Value = extractedAddress;

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

        public void Othersteps(List<string> rowdata, int row)
        {
            loginPage.EnterTittleNumberFromExcel(rowdata[3]);

            loginPage.ClickNextButton();
            var actualAddress = loginPage.GetActualAddress();
            Assert.That(actualAddress.Contains(rowdata[1]), Is.EqualTo(true));
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
            WriteDataToExcelSpreadSheet("Feb 2024", "J" + row, address);
            var appRef = loginPage.GetDisplayedApplicationReference();
            WriteDataToExcelSpreadSheet("Feb 2024", "K" + row, appRef);
            WriteDataToExcelSpreadSheet("Feb 2024", "M" + row, "Submitted");
            loginPage.ClickeDs1Discharge();
        }
    }
}

