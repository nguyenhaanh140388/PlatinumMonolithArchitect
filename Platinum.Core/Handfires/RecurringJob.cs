using Hangfire;

namespace Anhny010920.Core.Handfires
{
    public class RecurringJobClient : JobBase
    {
        protected readonly IRecurringJobManager _recurringJobManager;
        public RecurringJobClient(IRecurringJobManager recurringJobManager)
        {
            _recurringJobManager = recurringJobManager;
        }
    }
}
