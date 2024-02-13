using BoDi;
using LandRegistryProject.Drivers;
using LandRegistryProject.Support;
using LandRegistryProject.Utilities;
using NUnit.Framework;
using OpenQA.Selenium;
using System.ComponentModel;

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
        IWebElement Usermane => driver.FindElement(By.Id("username"));
        IWebElement Password => driver.FindElement(By.Id("password"));
        IWebElement LoginButton => driver.FindElement(By.XPath("//*[@type='submit']"));
        IWebElement TerminateExistingLog => driver.FindElement(By.XPath("//*[contains(text(),'Terminate existing login')]"));
        IWebElement Cookies => driver.WaitAndFindElement(By.Id("accept-cookies"));
        IWebElement LandingPage => driver.FindElement(By.XPath("//*[@class='govuk-heading-l']"));
        IWebElement RequestOfficialCop => driver.WaitAndFindElement(By.Id("position-3-link"));
        IWebElement LenderServices => driver.WaitAndFindElement(By.Id("lenderservices_link"));
        IWebElement eDs1Discharge => driver.FindElement(By.Id("ds1_link")); 
        IWebElement TittleNumber => driver.FindElement(By.CssSelector("#titleNumber"));
        IWebElement ActualAddress => driver.WaitAndFindElement(By.XPath("//div[@class='leftPanel']"));
        IWebElement NextButton => driver.WaitAndFindElement(By.XPath("(//*[@type='submit'])[2]"));
        IWebElement DateOfCharge => driver.FindElement(By.Id("chargeDate"));
        IWebElement YesRadioButton => driver.FindElement(By.Id("yesRedemption"));
        IWebElement YesMessagesOption => driver.FindElement(By.Id("yesMessage"));
        IWebElement CustomerReference => driver.WaitAndFindElement(By.Id("custRef"));
        IWebElement e_DS1Discharge => driver.FindElement(By.CssSelector("#formSubHeader")); //Title number: HD602384, 31 Wallace Green Way, Walkern, Stevenage (SG2 7FB)
        IWebElement applicationReference => driver.FindElement(By.CssSelector("[class='leftPanel preLinks1']")); //Application reference: HD602384/K190ZGT/206
        IWebElement LogoutButton => driver.FindElement(By.Id("portalLogoutLink"));
        

        public void EnterUsername()
        {
            Usermane.SendKeys(Config.Username);
        }

        public void EnterPassword()
        {
            Password.SendKeys(Config.Password);
        }

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

        public void ClickCookiesButton()
        {
            Cookies.Click();
        }

        public void ClickRequestOfficialCop()
        {
            RequestOfficialCop.Click();
        }

        public void ClickLenderServices()
        {
            LenderServices.Click();
        }

        public void ClickeDs1Discharge()
        {
            eDs1Discharge.Click();  
        }

        public string GetActualAddress()
        {
            return ActualAddress.Text;
        }

        public void EnterTittleNumberFromExcel(string Value)
        {
            TittleNumber.SendKeys(Value);
        }

        public void ClickNextButton()
        {
            NextButton.Click();
        }

        public void EnterDateOfCharge()
        {
            DateOfCharge.SendKeys(Config.dateOfcharge);
        }

        public void SelectYesRadioButton()
        {
            YesRadioButton.Click();
        }

        public bool IsYesMessagesOptionTicked() => YesMessagesOption.Selected;

        public void EnterCustomerReference()
        {
            CustomerReference.SendKeys(Config.Customer_Reference);
        }

        public string GetDisplayedAddressDetails()
        {
            return e_DS1Discharge.Text.Split(":")[1].TrimStart();
        }

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

        public void ClickLogoutButton()
        {
            LogoutButton.Click();
        }

       
    }
}
