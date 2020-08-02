using System;
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NLog;
using OpenQA.Selenium;

namespace PracticalTest.Common
{
    public class TestBase
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        protected string WebUrl;
        protected string ApiUrl;
        private string _driverType;
        public TestContext TestContext { get; set; }

        [TestInitialize]
        public void SetupTest()
        {
            WebUrl = ConfigurationManager.AppSettings["WebUrl"];
            ApiUrl = ConfigurationManager.AppSettings["ApiUrl"];
            _driverType = TestContext.Properties["Browser"]?.ToString();

            Logger.Info(DateTime.Now + $" - Test started on web ({WebUrl}) and api ({ApiUrl}). Driver is set to {_driverType}.");
        }

        public IWebDriver BuiDriver()
        {
            return DriverInitializer.StartDriver(_driverType, WebUrl);
        }
    }
}