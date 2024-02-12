using BoDi;
using Gherkin.Ast;
using LandRegistryProject.Drivers;
using LandRegistryProject.PageObject;
using LandRegistryProject.Support;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Linq;
using TechTalk.SpecFlow;

namespace LandRegistryProject.StepDefinitions
{
    [Binding]
    public class LoginPageStepDefinitions : DriverHelper
    {
        LoginPage loginPage;
        ReadingFromExcelSheet readingFromExcelSheet;

        public LoginPageStepDefinitions(IObjectContainer container)
        {
            driver = container.Resolve<IWebDriver>();
            loginPage = container.Resolve<LoginPage>();
            readingFromExcelSheet = container.Resolve<ReadingFromExcelSheet>();
        }
        [Given(@"I am on land registry sign-in page")]
        public void GivenIAmOnLandRegistrySign_InPage()
        {
            loginPage.GoToLandRegistryPage();
        }

        [When(@"I enter Username in the user name field")]
        public void WhenIEnterInTheUserNameField()
        {
            loginPage.EnterUsername();
        }

        [When(@"I enter Password in the password field")]
        public void WhenIEnterInThePasswordField()
        {
            loginPage.EnterPassword();
        }

        [Then(@"I click the login button")]
        public void ThenIClickTheLoginButton()
        {
                loginPage.ClickLogin();
        }


        [Then(@"data successfully sumitted")]
        public void ThenDataSuccessfullySumitted()
        {
            var Landingpage = loginPage.IsTextOnLandingPageDisplayed();
            Assert.That(Landingpage, Is.EqualTo(Landingpage));
            loginPage.ClickCookiesButton();
        }


        [Then(@"I click request official copies go to service button")]
        public void ThenIClickRequestOfficialCopiesGoToServiceButton()
        {
            loginPage.ClickRequestOfficialCop();
        }

        [Then(@"I click lender services")]
        public void ThenIClickLenderServices()
        {
            loginPage.ClickLenderServices();
        }

      // Scenario2

        [Given(@"I am on e-DS(.*) discharge page")]
        public void GivenIAmOnE_DSDischargePage(int p0)
       
        {
            loginPage.ClickeDs1Discharge();
        }

        [When(@"I am on e-DS(.*) discharge page")]
        public void WhenIAmOnE_DSDischargePage(int p0)
        {
            loginPage.ClickeDs1Discharge();
        }


        [When(@"I have identified address on excel datasheet and select the title number")]
        public void WhenIHaveIdentifiedExcelDataSheetAndEnterTheTitleNumber()
        {
            var address = readingFromExcelSheet.ReadRowData("D1:D5"); //From excel
            var titleNumber = readingFromExcelSheet.ReadRowData("I1:I5"); //From excel

            loginPage.EnterTittleNumberFromExcel(titleNumber[4]);
            loginPage.ClickNextButton();
            var actualAddress = loginPage.GetActualAddress();
            Assert.That(actualAddress.Contains(address[4]), Is.EqualTo(true));
        }

        [When(@"I click next button")]
        public void WhenIClickNextButton()
        {
            loginPage.ClickNextButton();
        }

        [When(@"I enter date of charge")]
        public void WhenIEnterDateOfCharge()
        {
            loginPage.EnterDateOfCharge();
        }

        [When(@"I select yes for  borrowers redemption button")]
        public void WhenISelectYesForBorrowersRedemptionButton()
        {
            loginPage.SelectYesRadioButton();
        }

        [When(@"I select yes for messages option")]
        public void WhenISelectYesForMessagesOption()
        {
            var YesMessagesOption = loginPage.IsYesMessagesOptionTicked();
            Assert.That(YesMessagesOption,Is.EqualTo(YesMessagesOption));
        }

        [When(@"I enter customer reference")]
        public void WhenIEnterCustomerReference()
        {
            loginPage.EnterCustomerReference();
        }

        [When(@"I click submit button")]
        public void WhenIClickSubmitButton()
        {
            loginPage.ClickNextButton();
        }

        [When(@"I copy the address to the excel data sheet")]
        public void WhenICopyTheAddressToTheExcelDataSheet()
        {
            var address = loginPage.GetDisplayedAddressDetails();
            readingFromExcelSheet.WriteDataToExcelSpreadSheet("Sheet1", "J5", address);
        }

        [When(@"I copy the application reference to the excel data sheet")]
        public void WhenICopyTheApplicationReferenceToTheExcelDataSheet()
        {
            var appRef = loginPage.GetDisplayedApplicationReference();
            readingFromExcelSheet.WriteDataToExcelSpreadSheet("Sheet1", "K5", appRef);
            readingFromExcelSheet.WriteDataToExcelSpreadSheet("Sheet1", "M5", "Submitted");
        }

        [Then(@"I click logout button")]
        public void ThenIClickLogoutButton()
        {
            loginPage.ClickLogoutButton();
        }


        [Then(@"I perform the end to end application submission for all the data")]
        public void ThenIPerformTheEndToEndApplicationSubmissionForAllTheData()
        {

            // readingFromExcelSheet.navigateSheet();
            readingFromExcelSheet.ReadExcelData();
            
        }

        


    }
}
