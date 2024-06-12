
using BoDi;
using LandRegistryProject.Drivers;
using LandRegistryProject.PageObject;

using NUnit.Framework;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OpenQA.Selenium;
using System.Security;
using System.Security.Policy;
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
            //File Path To BorrowBox_DS1File Path Location
            //ConnectToSharePointOnline();
            excelFilePath = projDir.Parent.Parent.Parent.FullName + @"\TestDatas\TitleNumber.xlsx";
            //excelFilePath = (Config.BorrowBox_DS1File_Path_Location);

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
            // String to access the worksheet
            List<string> cellValue = new List<string>();

            // Access the worksheet (Feb 2024 in this case)
            sheet = pck.Workbook.Worksheets[0];

            // Read data from Cells
            sheet.Cells[cellRange].ToList().ForEach(x => cellValue.Add(x.Value.ToString()!));

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
            //File Path To BorrowBox_DS1File Path Location
            excelFilePath = projDir.Parent.Parent.Parent.FullName + @"\TestDatas\TitleNumber.xlsx";
            //excelFilePath = (Config.BorrowBox_DS1File_Path_Location);

            //var ValueToUpdate = value.Length > 20 ? GetSubString(value) : value;
            var ValueToUpdate = value;
            sheet = pck.Workbook.Worksheets[0];
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
            try
            {
                bool cond = true;
                if (rowdata.GetValueOrDefault("HMLR Title No.").Equals("") && rowdata.GetValueOrDefault("Full Asset Address").Equals(""))
                {
                    cond = false;
                }
                else
                {
                    loginPage.EnterTittleNumberFromExcel(rowdata.GetValueOrDefault("HMLR Title No."));
                    loginPage.ClickNextButton();
                    var actualAddress = loginPage.GetActualAddress();
                    string expectedAddress = rowdata.GetValueOrDefault("Full Asset Address");
                    if (!compareAddress(actualAddress, expectedAddress))
                    {
                        cond = false;
                    }

                }
                if (cond)
                {

                    loginPage.ClickNextButton();
                    loginPage.EnterDateOfCharge();
                    loginPage.SelectYesRadioButton();
                    var YesMessagesOption = loginPage.IsYesMessagesOptionTicked();
                    //Assert.That(YesMessagesOption, Is.EqualTo(YesMessagesOption));
                    loginPage.ClickNextButton();
                    loginPage.ClickNextButton();
                    loginPage.EnterCustomerReference();
                    loginPage.ClickNextButton();
                    var address = loginPage.GetDisplayedAddressDetails();
                    WriteDataToExcelSpreadSheet(row, GetColumnByName("HMLR Asset Address"), address);
                    var appRef = loginPage.GetDisplayedApplicationReference();
                    WriteDataToExcelSpreadSheet(row, GetColumnByName("Discharge Reference"), appRef);
                    WriteDataToExcelSpreadSheet(row, GetColumnByName("eDS1 Status"), "Submitted");
                }

                else
                {
                    Console.WriteLine("\nManual Intervention required: " + row + "\n");
                    WriteDataToExcelSpreadSheet(row, GetColumnByName("eDS1 Status"), "Manual Intervention required");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nManual Intervention required: " + row + "\n");
                WriteDataToExcelSpreadSheet(row, GetColumnByName("eDS1 Status"), "Manual Intervention required");
            }
            loginPage.ClickeDs1Discharge();
        }

        private bool compareAddress(string actualAddress, string? expectedAddress)
        {
            string housenumber = "";
            string postcode = "";

            int firstSpaceIndex = expectedAddress.IndexOf(" ");
            int lastSpaceIndex = expectedAddress.LastIndexOf(" ");
            int postcodeSpaceIndex = expectedAddress.LastIndexOf(" ", lastSpaceIndex - 1);

            housenumber = expectedAddress.Substring(0, firstSpaceIndex).Trim();
            postcode = expectedAddress.Substring(postcodeSpaceIndex, expectedAddress.Length - postcodeSpaceIndex).Replace("(", "").Replace(")", "").Trim();
            //postcode = expectedAddress.Substring(40, 8).Trim();
            if (actualAddress.Contains(housenumber) && actualAddress.Contains(postcode))
                return true;
            else
                return false;

        }

    }
}