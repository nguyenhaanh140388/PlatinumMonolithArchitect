using System.Threading.Tasks;

namespace Anhny010920.Core.Abstractions.AzureService
{
    public interface IMessageBusFactory
    {
        IQueueBus GetAdminClient(string connectionString);
        Task<IMessageBus> GetClient(string connectionString, string senderName);
    }
}
