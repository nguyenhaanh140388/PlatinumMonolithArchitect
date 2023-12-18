using Azure.Messaging.ServiceBus.Administration;
using System.Threading.Tasks;

namespace Anhny010920.Core.Abstractions.AzureService
{
    public interface IQueueBus
    {
        Task<QueueProperties> CreateQueueAsync(string queueName, bool requiresSession = false);
        Task<QueueProperties> GetQueueAsync(string queueName);
        Task<Azure.Response> DeleteQueueAsync(string queueName);
    }
}
