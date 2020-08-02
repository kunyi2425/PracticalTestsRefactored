using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using PracticalTest.Common;
using PracticalTest.Objects;

namespace PracticalTest.Tests.Api
{
    [TestClass]
    [TestCategory("API"), TestCategory("SMOKE")]
    public class ApiTestRestApiTest : TestBase
    {
        private ApiCalculator _calculator;
        
        [TestInitialize]
        public void Setup()
        {
            _calculator = new ApiCalculator(ApiUrl);
        }

        [TestMethod]
        [DataRow("")]
        [DataRow("^")]
        [DataRow("12345")]
        public void ApiTestRestApiTest_OperatorInvalidOrEmpty404(string operatorUsed)
        {
            //Send api request with no operator
            var body = new
            {
                LeftNumber = 1,
                RightNumber = 2,
                Operator = operatorUsed 
            };
            var jsonBody = JsonConvert.SerializeObject(body);
            var response = _calculator.Authenticate()
                .SendRequest(jsonBody);

            //Verify server replies 404 Not found
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public void ApiTestRestApiTest_Unauthorized401()
        {
            //Send request with no auth
            var response = _calculator.SendRequest("");

            //Verify server replies 401 Unauthorized
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode );
        }

        [TestMethod]
        [DataRow(3.1415926, 10)]
        [DataRow(10, 0.0)]
        public void ApiTestRestApiTest_InputNotInteger500(double leftNumber, double rightNumber)
        {
            var body = new
            {
                leftNumber,
                rightNumber,
                Operator = '+'
            };
            var jsonBody = JsonConvert.SerializeObject(body);
            var response = _calculator.Authenticate()
                .SendRequest(jsonBody);

            //Verify server replies 500 internal server error
            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
        }
    }
}