using System.Security.Authentication;
using PracticalTest.Interfaces;
using OpenQA.Selenium;
using PracticalTest.Common;

namespace PracticalTest.Objects
{
    //A page object in the form of calculator
    public class WebCalculator : CommonWebBase, ICalculator
    {
        private readonly By _leftNumberTexBox = By.Id("leftNumber");
        private readonly By _rightNumberTextBox = By.Id("rightNumber");
        private readonly By _resultTextBox = By.ClassName("result");
        private readonly By _operatorList = By.Id("operator");
        private readonly By _iFrame = By.TagName("iframe");
        private readonly By _calculateButton = By.Id("calculate");
        private string _result;

        public WebCalculator(IWebDriver driver) : base(driver)
        {
        }

        public int Calculate(int leftNumber, int rightNumber, char operatorUsed)
        {
            return this.SetLeftNumber(leftNumber)
                .SetRightNumber(rightNumber)
                .SetOperator(operatorUsed)
                .ClickCalculateButton()
                .GetResult();
        }

        private WebCalculator SetLeftNumber(int leftNumber)
        {
            SwitchToDefaultContent();
            InputText(_leftNumberTexBox, leftNumber.ToString());
            return this;
        }

        private WebCalculator SetRightNumber(int rightNumber)
        {
            SwitchToDefaultContent();
            InputText(_rightNumberTextBox, rightNumber.ToString());
            return this;
        }

        private WebCalculator ClickCalculateButton()
        {
            SwitchToIFrame(_iFrame);
            ClickElement(_calculateButton);
            return this;
        }

        private WebCalculator SetOperator(char operatorUsed)
        {
            SwitchToDefaultContent();
            SelectItemInDropDown(_operatorList,operatorUsed.ToString());
            return this;
        }

        private int GetResult()
        {
            Driver.SwitchTo().DefaultContent();

            //Get the non-empty value from result element when the 1st time calculator is used
            //Otherwise, get the new value that is different from previous result
            _result = _result == null 
                ? GetValueFromElement(_resultTextBox)
                : GetDifferentValueFromElement(_resultTextBox, _result);

            var parsedSucceed = int.TryParse(_result, out var resultAsInt);

            //Calculator needs to return an integer, otherwise we have to stop our test and report an error
            return parsedSucceed
                ? resultAsInt
                : throw new AuthenticationException("Result value returned from Web is {_result}" +
                                                    "which is not an integer as expected.");
        }
    }
}