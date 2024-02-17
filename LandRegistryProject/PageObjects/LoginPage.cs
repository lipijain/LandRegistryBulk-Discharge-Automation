using BoDi;
using LandRegistryProject.Drivers;
using LandRegistryProject.Utilities;
using OpenQA.Selenium;

namespace LandRegistryProject.PageObject
{
    public class LoginPage : DriverHelper
    {
        public IObjectContainer _container;
        public LoginPage(IObjectContainer container)
        {
            driver = container.Resolve<IWebDriver>();
            _container = container; 
        }


        public void GoToLandRegistryPage() => driver.Navigate().GoToUrl(Config.url);
        private IWebElement Usermane => driver.FindElement(By.Id("username"));
        private IWebElement Password => driver.FindElement(By.Id("password"));
        private IWebElement LoginButton => driver.FindElement(By.XPath("//*[@type='submit']"));
        private IWebElement TerminateExistingLog => driver.FindElement(By.XPath("//*[contains(text(),'Terminate existing login')]"));
        private IWebElement Cookies => driver.WaitAndFindElement(By.Id("accept-cookies"));
        private IWebElement LandingPage => driver.FindElement(By.XPath("//*[@class='govuk-heading-l']"));
        private IWebElement RequestOfficialCop => driver.WaitAndFindElement(By.Id("position-3-link"));
        private IWebElement LenderServices => driver.WaitAndFindElement(By.Id("lenderservices_link"));
        private IWebElement eDs1Discharge => driver.FindElement(By.Id("ds1_link")); 
        private IWebElement TittleNumber => driver.FindElement(By.CssSelector("#titleNumber"));
        private IWebElement ActualAddress => driver.WaitAndFindElement(By.XPath("//div[@class='leftPanel']"));
        private IWebElement NextButton => driver.WaitAndFindElement(By.XPath("(//*[@type='submit'])[2]"));
        private IWebElement DateOfCharge => driver.FindElement(By.Id("chargeDate"));
        private IWebElement YesRadioButton => driver.FindElement(By.Id("yesRedemption"));
        private IWebElement YesMessagesOption => driver.FindElement(By.Id("yesMessage"));
        private IWebElement CustomerReference => driver.WaitAndFindElement(By.Id("custRef"));
        private IWebElement e_DS1Discharge => driver.FindElement(By.CssSelector("#formSubHeader")); 
        private IWebElement applicationReference => driver.FindElement(By.CssSelector("[class='leftPanel preLinks1']")); 
        private IWebElement LogoutButton => driver.FindElement(By.Id("portalLogoutLink"));


        public void EnterUsername() => Usermane.SendKeys(Config.Username);

        public void EnterPassword() => Password.SendKeys(Config.Password);

        public void ClickLogin() 
        {
            LoginButton.Click();
            try
            {
                if (driver.IsElementDisplayed(TerminateExistingLog))
                {
                    TerminateExistingLog.Click();
                }
            }
            catch (NoSuchElementException)
            {
                Console.WriteLine("Element not found");
            }
        }

        public bool IsTextOnLandingPageDisplayed() => LandingPage.Displayed;

        public void ClickCookiesButton() => Cookies.Click();

        public void ClickRequestOfficialCop() => RequestOfficialCop.Click();

        public void ClickLenderServices() => LenderServices.Click();

        public void ClickeDs1Discharge() => eDs1Discharge.Click();

        public string GetActualAddress() => ActualAddress.Text;

        public void EnterTittleNumberFromExcel(string Value) => TittleNumber.SendKeys(Value);

        public void ClickNextButton() => NextButton.Click();

        public void EnterDateOfCharge() => DateOfCharge.SendKeys(Config.dateOfcharge);

        public void SelectYesRadioButton() => YesRadioButton.Click();

        public bool IsYesMessagesOptionTicked() => YesMessagesOption.Selected;

        public void EnterCustomerReference() => CustomerReference.SendKeys(Config.Customer_Reference);

        public string GetDisplayedAddressDetails() => e_DS1Discharge.Text.Split(":")[1].TrimStart();

        public string GetDisplayedApplicationReference()
        {
            var displayedReferenceText = applicationReference.Text.Split("\r\n")[5].Split(":")[1].TrimStart();
            return displayedReferenceText;
        }

        public IWebElement getTextByJs(IWebElement element)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].innerText", element);
            return js.As<IWebElement>();
        }

        public void ClickLogoutButton() => LogoutButton.Click();
    }
}