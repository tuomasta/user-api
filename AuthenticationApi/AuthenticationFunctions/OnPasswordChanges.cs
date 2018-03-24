using DataAccess;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace OnPasswordChanges
{
    public static class OnPasswordChanges
    {
        [FunctionName("OnPasswordChanges")]
        public static void Run([ServiceBusTrigger("passwords", Connection = "ServiceBusConnection")]string passwordChangeJson, TraceWriter log)
        {
            var authInfo = JsonConvert.DeserializeObject<AuthentincationInfo>(passwordChangeJson);
            log.Info($"C# Queue trigger function processed: {passwordChangeJson}");

            // store only a has of the password
            authInfo.Password = authInfo.Password.ToSHA256Hash();

            var repo = new AuthRepo(ConnectionString, DatabaseName, CollectionName);
            repo.CreateAuthentication(authInfo);
        }



        static public string ConnectionString => ConfigurationManager.AppSettings.Get("AuthDbConnection");

        public static string DatabaseName => ConfigurationManager.AppSettings.Get("AuthDb"); // Environment.GetEnvironmentVariable("AuthDb", EnvironmentVariableTarget.Process);
        public static string CollectionName => ConfigurationManager.AppSettings.Get("AuthCollection");
    }
}
