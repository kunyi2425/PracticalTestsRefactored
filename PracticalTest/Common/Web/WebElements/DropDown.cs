using OpenQA.Selenium;
using PracticalTest.Common.Common;

namespace PracticalTest.Common.Web.WebElements
{
    public class DropDown : CommonWebElement
    {
        public DropDown(IWebDriver driver, By by) : base(driver, by)
        {
        }

        public void SelectValue(string value)
        {
            if (!ItemInDropDown(value))
                throw new AutomationException($"Value is not selectable in dropdown {By}");
            SelectItemInDropDown(value);
        }
    }
}