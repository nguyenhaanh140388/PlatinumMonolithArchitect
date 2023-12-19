using Platinum.Core.Abstractions.AzureService;
using Azure;
using Azure.Storage.Queues;
using ServiceStack.Text;

namespace Platinum.Infrastructure.Services.Azure
{
    public class AzureQueueStorageService : IQueueStorage
    {
        private readonly QueueClient _client;

        public AzureQueueStorageService(QueueClient client) { _client = client; }

        public async Task<Response> CreateQueue(string queueName)
        {
            return await _client.CreateIfNotExistsAsync();
        }

        public async Task<Response<bool>> DeleteQueue(string queueName)
        {
            return await _client.DeleteIfExistsAsync();
        }

        public async Task PublishMessageAsync<T>(T message)
        {
            var jsonMessage = JsonSerializer.SerializeToString(message);
            await _client.SendMessageAsync(jsonMessage);
        }

        public static async Task<Response> CreateQueueAsync(string connectionString, string queueName)
        {
            var client = new QueueClient(connectionString, queueName, new QueueClientOptions()
            {
                MessageEncoding = QueueMessageEncoding.Base64,
            });

            return await client.CreateIfNotExistsAsync();
        }

        public static IQueueStorage Create(QueueClient client)
        {
            return new AzureQueueStorageService(client);
        }
    }
}
