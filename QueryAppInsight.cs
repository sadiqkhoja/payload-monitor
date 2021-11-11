using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Configuration;

namespace PayloadMonitor
{
    public class QueryAppInsight
    {
        private readonly HttpClient _http;
        private readonly IAuthenticationService _authService;
        private readonly IConfiguration _config;

        public QueryAppInsight(HttpClient httpClient, IAuthenticationService authService, IConfiguration config)
        {
            _http = httpClient;
            _authService = authService;
            _config = config;
        }

        [FunctionName("QueryAppInsight")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var username = req.Headers["username"];
            var password = req.Headers["password"];

            if (!_authService.CheckCredentials(username, password))
            {
                return new ContentResult()
                {
                    StatusCode = 401,
                    Content = "'Invalid Credentials'"
                };
            }

            string environment = req.Query["environment"];
            string query = req.Query["query"];


            var appId = _config.GetValue<string>("appId_" + environment); 
            var appKey = _config.GetValue<string>("appKey_" + environment); 

            var url = $"https://api.applicationinsights.io/v1/apps/{appId}/query?timespan=PT4H&query={query}";

            var request = new HttpRequestMessage(HttpMethod.Get, url);

            request.Headers.Add("x-api-key", appKey);

            var response = await _http.SendAsync(request);

            var responseBody = await response.Content.ReadAsStringAsync();

            //JObject respJObj = JObject.Parse(responseBody);

            //var result = respJObj.Value<string>("result");

            return new OkObjectResult(responseBody);
        }
    }
}
