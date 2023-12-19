namespace Platinum.Core.Handfires
{
    public class BackgroundJob : JobBase
    {
        private readonly IBackgroundJobClient _backgroundJobClient;

        public BackgroundJob(IBackgroundJobClient backgroundJobClient)
        {
            _backgroundJobClient = backgroundJobClient;
        }
    }
}
