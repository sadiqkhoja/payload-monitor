using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace PayloadMonitor
{
    public class ValidateCredentials
    {
        private readonly IAuthenticationService _authService;

        public ValidateCredentials(IAuthenticationService authService)
        {
            _authService = authService;
        }

        [FunctionName("ValidateCredentials")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            string username = data?.username;
            string password = data?.password;

            if (_authService.CheckCredentials(username, password))
            {
                return new OkResult();                
            }

            return new ContentResult()
            {
                StatusCode = 401,
                Content = "Invalid Credentials"
            };

        }
    }
}
