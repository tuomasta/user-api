using Microsoft.ServiceBus.Messaging;
using Microsoft.ServiceBus;
using UserApi.Controllers;
using System.Threading.Tasks;
using System.Configuration;
using Newtonsoft.Json;

namespace UserApi.Messaging
{
    public class MessageBus : IMessageBus
    {
        // Thread-safe. Recommended that you cache rather than recreating it
        // on every request.
        private QueueClient passwordQueue;
        private static readonly string passwordQueueConnectionString = ConfigurationManager.AppSettings.Get("PasswordQueueConnectionString");

        public MessageBus(QueueClient passwordQueue)
        {
            this.passwordQueue = passwordQueue;
        }

        public async Task SendEventAsync(IUserEvent @event) {

            // own handling for created event as we don't want to broadcast the plain password
            if (@event is UserCreatedEvent created)
            {
                var messageContent = JsonConvert.SerializeObject(new { created.UserId, created.Password, created.Email });
                await passwordQueue.SendAsync(new BrokeredMessage(messageContent));

                // note side effect
                created.Password = null;
            }

            // .. send events topic

        }

        public static IMessageBus Configure()
        {
            // Using Http to be friendly with outbound firewalls.
            ServiceBusEnvironment.SystemConnectivity.Mode = ConnectivityMode.Http;
            var passwordQueue = QueueClient.CreateFromConnectionString(passwordQueueConnectionString);
            return new MessageBus(passwordQueue);
        }
    }
}