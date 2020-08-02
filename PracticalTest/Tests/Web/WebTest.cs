using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using PracticalTest.Common;
using PracticalTest.Interfaces;
using PracticalTest.Objects;

namespace PracticalTest.Tests.Web
{
    [TestClass]
    [TestCategory("WEB")]
    public class WebTest : TestBase
    {
        private IWebDriver _driver;
        private ICalculator _calculator;

        [TestInitialize]
        public void Setup()
        {   
            _driver = BuiDriver();
            _driver.Url = WebUrl;
            _calculator = new WebCalculator(_driver);
        }

        [TestMethod]
        [DataRow(999, 999, '+', 999 + 999)]
        [DataRow(999, 999, '-', 0)]
        [DataRow(999, 7, '*', 999 * 7)]
        [DataRow(999, 34, '/', 999 / 34)]
        [DataRow(0, -999, '*', 0)]
        public void WebTest_PositiveTests(int leftNumber, int rightNumber, char operatorUsed, int expectedResult)
        {
            var actualResult = _calculator.Calculate(leftNumber, rightNumber, operatorUsed);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        //All these cases are supposed to be wrong or cause a customized error
        [DataRow(-99, 99, '-', -99 - 99)]
        [DataRow(999, 999, '*', 999 * 999)]
        [DataRow(1, 0, '/', 999 * 999)]
        public void WebTest_NegativeTests(int leftNumber, int rightNumber, char operatorUsed, int expectedResult)
        {
            var actualResult = _calculator.Calculate(leftNumber, rightNumber, operatorUsed);
            //Assert for not equal for now - can be raised as bug
            Assert.AreNotEqual(expectedResult, actualResult);
        }

        [TestMethod]
        //Verify if we can test a sequence of calculations in 1 go - with 1 driver
        //This is to make sure each result is not impacted by previous calculations - calculator is stable
        public void WebTest_MultipleCalculations()
        {
            var actualResult = _calculator.Calculate(1, 1, '+');
            Assert.AreEqual(2, actualResult, "1st calculation failed.");

            actualResult = _calculator.Calculate(2, 2, '*');
            Assert.AreEqual(4, actualResult, "2nd calculation failed.");

            actualResult = _calculator.Calculate(9, 3, '/');
            Assert.AreEqual(3, actualResult, "3rd calculation failed.");

            actualResult = _calculator.Calculate(5, 5, '-');
            Assert.AreEqual(0, actualResult, "4th calculation failed.");
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _driver.Close();
        }
    }
}