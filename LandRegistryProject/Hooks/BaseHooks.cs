using BoDi;
using LandRegistryProject.Drivers;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using TechTalk.SpecFlow;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

namespace LandRegistryProject.Hooks
{
    [Binding]
    public sealed class BaseHooks :DriverHelper
    {
        IObjectContainer container;
        
        public BaseHooks(IObjectContainer objectContainer)
        {
            container = objectContainer;   
        }

        [BeforeScenario]
        public void BeforeScenarioWithTag()
        {
            new DriverManager().SetUpDriver(new ChromeConfig());
            ChromeOptions options = new ChromeOptions();
            options.AddArguments("--start-maximized", "incognito");
            driver = new ChromeDriver(options);
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(20);
            container.RegisterInstanceAs(driver);

        }

        [AfterScenario]
        public void AfterScenario()
        {
            Thread.Sleep(3000);
            if (driver != null)
            {
                driver.Quit();
            }
            driver = null;
        }
    }
}