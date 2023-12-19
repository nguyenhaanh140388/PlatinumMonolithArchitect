using Platinum.Core.Abstractions.Services;

namespace Platinum.Infrastructure.Services
{
    public class DateTimeService : IDateTimeService
    {
        public DateTime NowUtc => DateTime.UtcNow;
    }
}
