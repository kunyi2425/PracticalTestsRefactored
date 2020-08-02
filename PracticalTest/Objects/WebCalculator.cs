using System.Security.Authentication;
using OpenQA.Selenium;
using PracticalTest.Common.Web.WebElements;
using PracticalTest.Interfaces;

namespace PracticalTest.Objects
{
    //A page object in the form of calculator
    public class WebCalculator : ICalculator
    {
        private readonly By _leftNumberTexBoxBy = By.Id("leftNumber");
        private readonly By _rightNumberTextBoxBy = By.Id("rightNumber");
        private readonly By _resultTextBoxBy = By.ClassName("result");
        private readonly By _operatorListBy = By.Id("operator");
        private readonly By _iFrameBy = By.TagName("iframe");
        private readonly By _calculateButtonBy = By.Id("calculate");
        private readonly Button _calculateButton;
        private readonly Textbox _leftNumberTextBox;
        private readonly Textbox _rightNumberTextBox;
        private readonly Textbox _resultTextBox;
        private readonly DropDown _operatorList;
        private string _result;

        public WebCalculator(IWebDriver driver)
        {
            _calculateButton = new Button(driver, _calculateButtonBy, _iFrameBy);
            _leftNumberTextBox = new Textbox(driver, _leftNumberTexBoxBy);
            _rightNumberTextBox = new Textbox(driver, _rightNumberTextBoxBy);
            _resultTextBox = new Textbox(driver, _resultTextBoxBy);
            _operatorList = new DropDown(driver, _operatorListBy);
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
            _leftNumberTextBox.EnterText(leftNumber.ToString());
            return this;
        }

        private WebCalculator SetRightNumber(int rightNumber)
        {
            _rightNumberTextBox.EnterText(rightNumber.ToString());
            return this;
        }

        private WebCalculator ClickCalculateButton()
        {
            _calculateButton.Click();
            return this;
        }

        private WebCalculator SetOperator(char operatorUsed)
        {
            _operatorList.SelectValue(operatorUsed.ToString());
            return this;
        }

        private int GetResult()
        {
            //Get the non-empty value from result element when the 1st time calculator is used
            //Otherwise, get the new value that is different from previous result
            _result = _result == null 
                ? _resultTextBox.GetPopulatedValue()
                : _resultTextBox.GetChangedValue(_result);

            var parsedSucceed = int.TryParse(_result, out var resultAsInt);

            //Calculator needs to return an integer, otherwise we have to stop our test and report an error
            return parsedSucceed
                ? resultAsInt
                : throw new AuthenticationException("Result value returned from Web is {_result}" +
                                                    "which is not an integer as expected.");
        }
    }
}