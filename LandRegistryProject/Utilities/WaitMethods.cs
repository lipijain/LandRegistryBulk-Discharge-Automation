using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace LandRegistryProject.Utilities
{
    public static class WaitMethods
    {
        public static IWebElement WaitAndFindElement(this IWebDriver driver, By locator)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));

            try
            {
                wait.Until(ExpectedConditions.ElementIsVisible(locator));
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine("Element is not found within 15 Sec");
            }

            return wait.Until(ExpectedConditions.ElementIsVisible(locator));
        }
    }
}
