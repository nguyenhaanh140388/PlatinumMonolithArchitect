using System.Threading.Tasks;

namespace Anhny010920.Core.Abstractions.AzureService
{
    public interface IMessageBus
    {
        Task PublishMessageAsync<T>(T message);
    }
}
