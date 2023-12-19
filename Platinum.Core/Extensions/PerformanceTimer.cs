using System.Diagnostics;

namespace Platinum.Core.Extensions
{
    /// <summary>
    /// Records how long an action or actions take to execute. Use this class in a using statement.
    /// </summary>
    public class PerformanceTimer : IDisposable
    {
        private readonly Stopwatch _stopwatch;
        private readonly IOnComplete<TimeSpan> _completionSubscriber;

        /// <summary>
        /// Constructs a new instance of the Performance timer.
        /// </summary>
        /// <param name="category">Optional category name to write to the Trace.Listeners collection.</param>
        public PerformanceTimer(string category = null)
        {
            _completionSubscriber = new TraceCompletionSubscriber(category);
            _stopwatch = Stopwatch.StartNew();
        }

        /// <summary>
        /// Constructs a new instance of the Performance timer.
        /// </summary>
        /// <param name="completionSubscriber">An optional class that implements <see cref="IOnComplete{TimeSpan}"/>IOnComplete</param>
        public PerformanceTimer(IOnComplete<TimeSpan> completionSubscriber = null)
        {
            _completionSubscriber = completionSubscriber ?? new TraceCompletionSubscriber(category: null);
            _stopwatch = Stopwatch.StartNew();
        }

        /// <summary>
        /// Stops the timer and notifies the associated completion subscriber.
        /// </summary>
        public void Dispose()
        {
            _stopwatch.Stop();
            _completionSubscriber?.OnComplete(_stopwatch.Elapsed);
        }

        private class TraceCompletionSubscriber : IOnComplete<TimeSpan>
        {
            private readonly string _category;

            public TraceCompletionSubscriber(string category)
            {
                _category = category;
            }

            public void OnComplete(TimeSpan data)
            {
                if (string.IsNullOrWhiteSpace(_category))
                {
                    Trace.WriteLine($"Execution took {data}.");
                }
                else
                {
                    Trace.WriteLine($"Execution took {data}.", _category);
                }
            }
        }
    }
}