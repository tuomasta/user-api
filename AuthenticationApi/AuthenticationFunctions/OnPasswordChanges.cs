using DataAccess;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using System.Configuration;

namespace OnPasswordChanges
{
    public static class OnPasswordChanges
    {
        [FunctionName("OnPasswordChanges")]
        public static void Run([ServiceBusTrigger("passwords", Connection = "ServiceBusConnection")]string passwordChangeJson, TraceWriter log)
        {
            var authInfo = JsonConvert.DeserializeObject<AuthentincationInfo>(passwordChangeJson);
            log.Info($"C# Queue trigger function processed: {passwordChangeJson}");

            // store only the hash of the password
            authInfo.Password = authInfo.Password.ToSHA256Hash();

            var repo = new AuthRepo(ConnectionString, DatabaseName, CollectionName);
            repo.CreateAuthentication(authInfo);
        }

        public static string ConnectionString => ConfigurationManager.AppSettings.Get("AuthDbConnection");
        public static string DatabaseName => ConfigurationManager.AppSettings.Get("AuthDb");
        public static string CollectionName => ConfigurationManager.AppSettings.Get("AuthCollection");
    }
}
