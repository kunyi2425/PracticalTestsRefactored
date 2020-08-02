using System;
using NLog;
using PracticalTest.Common.Common;
using RestSharp;

namespace PracticalTest.Common.Api
{
    public abstract class CommonApiBase
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly RestRequest _restRequest = new RestRequest();
        private readonly string _url;

        protected CommonApiBase(string baseUrl, Method method)
        {
            _url = baseUrl;
            _restRequest.Method = method;
        }

        private void AddOrUpdateHeader(string key, string value)
        {
            _restRequest.AddOrUpdateParameter(key, value, ParameterType.HttpHeader);
        }

        protected void SetAuthenticationHeader(string token)
        {
            AddOrUpdateHeader("x-functions-key", token);
        }

        protected void AddJsonBody(string jsonString)
        {
            _restRequest.AddOrUpdateParameter("Application/Json", jsonString, ParameterType.RequestBody);
        }

        protected IRestResponse ExecuteCall()
        {
            try
            {
                var restClient = new RestClient(_url);
                var response = restClient.Execute(_restRequest);

                Logger.Info(DateTime.Now + " - request to " + _url + " had as parameters: "
                            + string.Join(", ", _restRequest.Parameters));
                Logger.Info(DateTime.Now + " - response is " + response.StatusCode + " " + response.Content);

                return response;
            }
            catch (Exception e)
            {
                throw new AutomationException("Failed to execute rest request because of error " + e.Message);
            }
        }
    }
}