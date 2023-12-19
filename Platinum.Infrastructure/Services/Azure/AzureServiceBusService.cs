using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using Platinum.Core.Abstractions.AzureService;
using ServiceStack.Text;
using System.Text;

namespace Platinum.Infrastructure.Services.Azure
{
    public class AzureServiceBusService : IMessageBus
    {
        private ServiceBusSender _sender;

        public AzureServiceBusService(ServiceBusSender sender)
        {
            _sender = sender;
        }

        public async Task PublishMessageAsync<T>(T message)
        {
            var jsonMessage = JsonSerializer.SerializeToString(message);
            var serviceBusEncodedMessage = new ServiceBusMessage(Encoding.UTF8.GetBytes(jsonMessage));
            await _sender.SendMessageAsync(serviceBusEncodedMessage);
        }

        public static async Task<QueueProperties> CreateQueueAsync(string connectionString, string queueName, bool requiresSession = false)
        {
            var client = new ServiceBusAdministrationClient(connectionString);
            var options = new CreateQueueOptions(queueName)
            {
                DefaultMessageTimeToLive = TimeSpan.FromDays(2),
                DuplicateDetectionHistoryTimeWindow = TimeSpan.FromMinutes(1),
                EnableBatchedOperations = true,
                DeadLetteringOnMessageExpiration = true,
                EnablePartitioning = false,
                ForwardDeadLetteredMessagesTo = null,
                ForwardTo = null,
                LockDuration = TimeSpan.FromSeconds(45),
                MaxDeliveryCount = 8,
                MaxSizeInMegabytes = 2048,
                UserMetadata = "some metadata",
                RequiresSession = requiresSession
            };

            options.AuthorizationRules.Add(new SharedAccessAuthorizationRule(
                "allClaims",
                new[] { AccessRights.Manage, AccessRights.Send, AccessRights.Listen }));

            return await client.CreateQueueAsync(options);
        }

        public static IMessageBus Create(ServiceBusSender sender)
        {
            return new AzureServiceBusService(sender);
        }
    }
}
