using Platinum.Core.Abstractions.Dtos;

namespace Platinum.Core.Abstractions.Services
{
    public interface IEmailService
    {
        Task SendAsync(EmailRequest request);
    }
}
