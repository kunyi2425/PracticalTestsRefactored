using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace PracticalTest.Common
{
    public abstract class CommonWebBase
    {
        protected IWebDriver Driver;
        
        protected CommonWebBase(IWebDriver driver)
        {
            Driver = driver;
        }

        protected IWebElement FindElement(By by)
        {
            return Driver.FindElement(by);
        }

        protected CommonWebBase ClickElement(By by)
        {
            FindElement(by).Click();
            return this;
        }

        protected CommonWebBase SelectItemInDropDown(By by, string value)
        {
            ClickElement(by);
            var select = new SelectElement(FindElement(by));
            select.SelectByValue(value);
            return this;
        }

        //Used to fetch value after it is populated
        protected string GetValueFromElement(By by)
        {
            WaitValueIsPopulatedOrChanged(by);
            return FindElement(by).GetAttribute("value");
        }

        //Used to fetch value after it is changed
        protected string GetDifferentValueFromElement(By by, string changeFromValue)
        {
            WaitValueIsPopulatedOrChanged(by, changeFromValue);
            return FindElement(by).GetAttribute("value");
        }

        protected CommonWebBase InputText(By by, string input)
        {
            FindElement(by).Clear();
            FindElement(by).SendKeys(input);
            return this;
        }

        protected void SwitchToIFrame(By frameBy)
        {
            Driver.SwitchTo().Frame(FindElement(frameBy));
        }

        protected void SwitchToDefaultContent()
        {
            Driver.SwitchTo().DefaultContent();
        }

        protected void WaitValueIsPopulatedOrChanged(By by, string changeFromValue = null)
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(60));

            if (changeFromValue == null)
            {
                wait.Until(d => FindElement(@by).GetAttribute("value") != "");
            }
            else
            {
                wait.Until(d => FindElement(@by).GetAttribute("value") != changeFromValue);
            }
        }
    }
}