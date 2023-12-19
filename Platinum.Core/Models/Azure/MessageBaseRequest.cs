using System;

namespace Platinum.Core.Models.Azure
{
    public class MessageBaseRequest
    {
        public string Queue { get; set; }

        public string Request { get; set; }

        public string MessageId { get; set; }

        public DateTimeOffset Timestamp { get; set; }

        public int Version { get; set; }

        public MessageBaseRequest()
        {
            Queue = GetType().Name;
            MessageId = Guid.NewGuid().ToString();
            Timestamp = DateTimeOffset.Now;
            Version = 1;
        }
    }
}
