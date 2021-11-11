using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PayloadMonitor
{
    public class AuthenticationService : IAuthenticationService
    {
        private Dictionary<string, string> users;

        public AuthenticationService(IConfiguration config)
        {
            var userStr = config.GetValue<string>("users");

            this.users = JsonConvert.DeserializeObject<Dictionary<string, string>>(userStr);
        }

        public bool CheckCredentials(string username, string password)
        {
            return users.ContainsKey(username) && users[username] == password;
        }
    }
}
