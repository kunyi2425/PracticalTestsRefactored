using OpenQA.Selenium;
using PracticalTest.Common.Common;

namespace PracticalTest.Common.Web.WebElements
{
    public class Textbox : CommonWebElement
    {
        public Textbox(IWebDriver driver, By by) : base(driver, by)
        {
        }

        public void EnterText(string input)
        {
            if(!PerformBasicHealthCheck())
                throw new AutomationException($"Textbox {By} is not found or visible on the page");
            SendText(input);
        }

        public string GetPopulatedValue()
        {
            return GetValueWhenPopulated();
        }

        public string GetChangedValue(string previousValue)
        {
            return GetValueWhenChanged(previousValue);
        }
    }
}