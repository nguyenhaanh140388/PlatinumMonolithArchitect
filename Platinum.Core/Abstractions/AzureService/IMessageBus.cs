using System.Threading.Tasks;

namespace Platinum.Core.Abstractions.AzureService
{
    public interface IMessageBus
    {
        Task PublishMessageAsync<T>(T message);
    }
}
