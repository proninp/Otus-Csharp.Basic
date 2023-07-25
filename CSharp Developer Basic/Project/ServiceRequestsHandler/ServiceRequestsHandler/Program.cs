using ServiceRequestsHandler.API;
using ServiceRequestsHandler.Models;

namespace ServiceRequestsHandler
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var userTest = new User(0, 0, Environment.GetEnvironmentVariable("API_TEST_LOGIN"));
            userTest.SetAuthPasswordTest(Environment.GetEnvironmentVariable("API_TEST_PASSWORD"));

            var api = new ServiceAPI();

            var authToken = await api.GetAuthenticationToken(userTest);
            userTest.SetAuthTokenTest(authToken);
            
            var serviceRequests = await api.GetServiceEngineersRequest(userTest);
            Console.WriteLine(serviceRequests);


            Console.ReadLine();
        }
    }
}