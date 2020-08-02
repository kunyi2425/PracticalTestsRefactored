using System.Configuration;
using System.Net;
using Newtonsoft.Json;
using PracticalTest.Common;
using PracticalTest.Interfaces;
using PracticalTest.Models;
using RestSharp;

namespace PracticalTest.Objects
{
    //An api object that implements a calculator
    public class ApiCalculator : CommonApiBase, ICalculator
    {
        private const Method Method = RestSharp.Method.POST;

        public ApiCalculator(string baseUrl) : base(baseUrl, Method)
        {
        }

        //Method should be used to test the functionality of the calculator not api standard
        public int Calculate(int leftNumber, int rightNumber, char operatorUsed)
        {
            var response = this.Authenticate()
                .AddBody(leftNumber, rightNumber, operatorUsed)
                .ExecuteCall();

            if(response.StatusCode != HttpStatusCode.OK)
                throw new AutomationException("Server replied failure, please check response in test log.");

            var responseObject = JsonConvert.DeserializeObject<ApiCalculatorResponse>(response.Content);
            return responseObject.Value;
        }

        //Method should be used for testing api standard
        public IRestResponse SendRequest(string jsonBody)
        {
            AddJsonBody(jsonBody);
            return ExecuteCall();
        }

        public ApiCalculator Authenticate()
        {
            var authToken = ConfigurationManager.AppSettings["AuthToken"];
            SetAuthenticationHeader(authToken);
            return this;
        }

        private ApiCalculator AddBody(int leftNumber, int rightNumber, char operatorUsed)
        {
            var requestBody = new ApiCalculatorRequest
            {
                LeftNumber = leftNumber,
                RightNumber = rightNumber,
                Operator = operatorUsed
            };
            AddJsonBody(JsonConvert.SerializeObject(requestBody));

            return this;
        }
    }
}