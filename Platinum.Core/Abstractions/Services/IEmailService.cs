using Platinum.Core.Abstractions.Dtos;
using System.Threading.Tasks;

namespace Platinum.Core.Abstractions.Services
{
    public interface IEmailService
    {
        Task SendAsync(EmailRequest request);
    }
}
