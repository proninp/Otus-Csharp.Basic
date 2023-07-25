using Newtonsoft.Json.Linq;
using ServiceRequestsHandler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceRequestsHandler.API
{
    public class ServiceAPI
    {
        private static readonly HttpClient HttpClient = new();
        public async Task<string> GetAuthenticationToken(User user)
        {
            try
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, Config.AuthApiUrl);
                request.Headers.Add("Accept", Config.AcceptHeader);
                request.Headers.Add("Authorization", user.GetAuthUserPasswordValue());

                var response = await HttpClient.SendAsync(request);
                var responseText = await response.Content.ReadAsStringAsync();
                var tokenValue = GetAuthApiResponseValue(responseText);
                return tokenValue;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString()); // TODO Logging
            }
            return "";
        }
        public async Task<string> GetServiceEngineersRequest(User user)
        {
            try
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, Config.ServiceRequestsListApiUrl);
                request.Headers.Add("Accept", Config.AcceptHeader);
                request.Headers.Add("Authorization", user.GetAuthTokenValue());

                var response = await HttpClient.SendAsync(request);
                var responseText = await response.Content.ReadAsStringAsync();
                var jsonResponse = GetApiResponseValue(responseText);
                return jsonResponse;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString()); // TODO Logging
            }
            return "";
        }
        private string? GetAuthApiResponseValue(string responseString)
        {
            // От бэка всегда приходит {"@odata.context", "value"}
            if (string.IsNullOrEmpty(responseString))
                return null;
            JObject jObject = JObject.Parse(responseString);
            return jObject["value"]?.Value<string>();

        }
        private string? GetApiResponseValue(string responseString)
        {
            // От бэка всегда приходит {"@odata.context", "value"}
            if (string.IsNullOrEmpty(responseString))
                return null;
            JObject jObject = JObject.Parse(responseString);
            var value = jObject["value"];
            return value?.ToString();
        }

    }
}
