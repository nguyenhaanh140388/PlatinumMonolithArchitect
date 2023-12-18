using Anhny010920.Core.Abstractions.Dtos;
using System.Threading.Tasks;

namespace Anhny010920.Core.Abstractions.Services
{
    public interface IEmailService
    {
        Task SendAsync(EmailRequest request);
    }
}
