using Microsoft.VisualStudio.TestTools.UnitTesting;
using PracticalTest.Common;
using PracticalTest.Interfaces;
using PracticalTest.Objects;

namespace PracticalTest.Tests.Api
{
    [TestClass]
    [TestCategory("API")]
    public class ApiTestCalculatorFunctionTests : TestBase
    {
        private ICalculator _calculator;
        
        [TestInitialize]
        public void Setup()
        {
            _calculator = new ApiCalculator(ApiUrl);
        }

        [TestMethod]
        //Test all operators plus some boundary tests
        [DataRow(int.MaxValue - 1, 1, '+', int.MaxValue)]
        [DataRow(int.MaxValue, int.MaxValue, '-', 0)]
        [DataRow(int.MaxValue, 3, '/', int.MaxValue / 3)]
        [DataRow(int.MaxValue / 3, 3, '*', int.MaxValue / 3 * 3)]
        [DataRow(int.MinValue, int.MaxValue, '+', int.MinValue + int.MaxValue)]
        [DataRow(-100, 5, '/', -20)]
        [DataRow(-99, 99, '-', -198)]
        public void ApiTest_PositiveTests(int leftNumber, int rightNumber, char operatorUsed, int expectedResult)
        {
            var actualResult = _calculator.Calculate(leftNumber, rightNumber, operatorUsed);
            Assert.AreEqual(actualResult, expectedResult);
        }

        [TestMethod]
        //When we add 1 to the largest integer, api returns -2147483648 which is the smallest int. Bug?
        public void ApiTest_NegativeTest_NumberTooBig()
        {
            var leftNumber = int.MaxValue;
            var rightNumber = 1;
            var operatorUsed = '+';
            var actualResult = _calculator.Calculate(leftNumber, rightNumber, operatorUsed);
            Assert.AreEqual(actualResult, -2147483648);
        }

        [TestMethod]
        //API may need to return a customized error other then throws 500 internal server error. Bug?
        public void ApiTest_NegativeTest_DividedByZero()
        {
            var leftNumber = 1;
            var rightNumber = 0;
            var operatorUsed = '/';
            var ex = Assert.ThrowsException<AutomationException>(
                () => _calculator.Calculate(leftNumber, rightNumber, operatorUsed));
            Assert.AreEqual(ex.Message, "Server replied failure, please check response in test log.");
        }
    }
}