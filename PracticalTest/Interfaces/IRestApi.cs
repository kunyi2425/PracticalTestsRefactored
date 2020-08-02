using RestSharp;

namespace PracticalTest.Interfaces
{
    interface IRestApi
    {
        IRestResponse SendRequest(string jsonBody);

        void Authenticate();
    }
}
