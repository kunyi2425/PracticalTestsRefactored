using System;
using System.Linq;
using NLog;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using static SeleniumExtras.WaitHelpers.ExpectedConditions;

namespace PracticalTest.Common.Web.WebElements
{
    public abstract class CommonWebElement
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public IWebElement Element;
        public By By;
        public IWebDriver Driver;
        private readonly WebDriverWait _wait;

        protected CommonWebElement(IWebDriver driver, By by)
        {
            Driver = driver;
            By = by;
            _wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(60));
        }

        protected bool IsVisible
        {
            get
            {
                try
                {
                    _wait.Until(ElementIsVisible(By));
                }
                catch
                {
                    Logger.Info($"Element {By} is not visible on the page");
                    return false;
                }

                return true;
            }
        }

        protected bool IsOnPage
        {
            get
            {
                try
                {
                    _wait.Until(ElementExists(By));
                }
                catch
                {
                    Logger.Info($"Element {By} cannot be found on the page");
                    return false;
                }

                return true;
            }
        }

        protected bool IsClickable
        {
            get
            {
                try
                {
                    _wait.Until(ElementToBeClickable(By));
                }
                catch
                {
                    Logger.Info($"Element {By} is not clickable");
                    return false;
                }

                return true;
            }
        }

        protected void ClickElement()
        {
            Driver.FindElement(By).Click();
        }

        protected void SendText(string text)
        {
            Driver.FindElement(By).SendKeys(text);
        }

        protected bool PerformBasicHealthCheck()
        {
            return IsVisible && IsOnPage;
        }

        protected bool ItemInDropDown(string itemValue)
        {
            var select = new SelectElement(Driver.FindElement(By));
            var options = select.Options;
            return options.Any(option => option.Text.Contains(itemValue));
        }

        protected void SelectItemInDropDown(string value)
        {
            ClickElement();
            var select = new SelectElement(Driver.FindElement(By));
            select.SelectByValue(value);
        }

        protected string GetValueWhenPopulated()
        {
            _wait.Until(d => Driver.FindElement(By).GetAttribute("value") != "");
            return Driver.FindElement(By).GetAttribute("value");
        }

        protected string GetValueWhenChanged(string orginalValue)
        {
            _wait.Until(d => Driver.FindElement(By).GetAttribute("value") != orginalValue);
            return Driver.FindElement(By).GetAttribute("value");
        }

        protected void SwitchToIframe(By iframeBy)
        {
            Driver.SwitchTo().Frame(Driver.FindElement(iframeBy));
        }

        protected void SwitchToDefault()
        {
            Driver.SwitchTo().DefaultContent();
        }
    }
}