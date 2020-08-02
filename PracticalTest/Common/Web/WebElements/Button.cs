using OpenQA.Selenium;
using PracticalTest.Common.Common;

namespace PracticalTest.Common.Web.WebElements
{
    public class Button : CommonWebElement
    {
        private By _iFrameBy;

        public Button(IWebDriver driver, By by, By frameLocator = null) : base(driver, by)
        {
            _iFrameBy = frameLocator;
        }

        public void Click()
        {
            if(_iFrameBy != null)
                SwitchToIframe(_iFrameBy);

            if(!IsClickable)
                throw new AutomationException($"Button {By} is not clickable on the page");
            ClickElement();

            if (_iFrameBy != null)
                SwitchToDefault();
        }
    }
}