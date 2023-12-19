using Azure;

namespace Platinum.Infrastructure.Services.Azure
{
    public class ServiceBusAdministrationService : IQueueBus
    {

        private readonly ServiceBusAdministrationClient _client;

        public ServiceBusAdministrationService(ServiceBusAdministrationClient client)
        {
            _client = client;
        }

        public async Task<QueueProperties> CreateQueueAsync(string queueName, bool requiresSession = false)
        {
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

            //options.AuthorizationRules.Add(new SharedAccessAuthorizationRule(
            //    "allClaims",
            //    new[] { AccessRights.Manage, AccessRights.Send, AccessRights.Listen }));

            if (!await _client.QueueExistsAsync(queueName))
            {
                return await _client.CreateQueueAsync(options);
            }

            return await _client.GetQueueAsync(queueName);
        }

        public async Task<QueueProperties> GetQueueAsync(string queueName)
        {
            try
            {
                return await _client.GetQueueAsync(queueName);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Response> DeleteQueueAsync(string queueName)
        {
            if (await _client.QueueExistsAsync(queueName))
            {
                return await _client.DeleteQueueAsync(queueName);
            }

            return new AzureResponse();
        }

        public static IQueueBus Create(ServiceBusAdministrationClient client)
        {
            return new ServiceBusAdministrationService(client);
        }
    }
}
