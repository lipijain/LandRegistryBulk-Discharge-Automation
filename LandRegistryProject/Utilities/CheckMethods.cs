using OpenQA.Selenium;

namespace LandRegistryProject.Utilities
{
    public static class CheckMethods
    {
        public static bool IsElementDisplayed(this IWebDriver driver, IWebElement element)
        {
            try
            {
                return element.Displayed;
            }
            catch (NoSuchElementException)
            {
               return false;
            }
        }
    }
}
