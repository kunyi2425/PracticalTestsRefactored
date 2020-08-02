using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using PracticalTest.Common.Common;

namespace PracticalTest.Common.Web
{
    public static class DriverInitializer
    {
        public static IWebDriver StartDriver(string driverType, string baseUrl)
        {
            if (string.IsNullOrEmpty(driverType))
                throw new AutomationException("Driver type is not provided. Please check test setting file.");

            switch(driverType.ToUpper())
            {
                case "CHROME":
                    return new ChromeDriver();
                case "IE":
                    var options = new InternetExplorerOptions();
                    options.IntroduceInstabilityByIgnoringProtectedModeSettings = true;
                    options.IgnoreZoomLevel = true;
                    options.InitialBrowserUrl = baseUrl;
                    var driver = new InternetExplorerDriver(options);
                    return driver;
                case "FIREFOX":
                    return new FirefoxDriver();
                default:
                    throw new AutomationException($"Failed initializing driver. " +
                                                  $"Given driver type {driverType} is not supported.");

            }
        }
    }
}