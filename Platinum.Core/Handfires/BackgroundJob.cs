using Hangfire;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Anhny010920.Core.Handfires
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
