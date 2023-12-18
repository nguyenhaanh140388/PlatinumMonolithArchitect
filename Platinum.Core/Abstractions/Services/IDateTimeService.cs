using System;
using System.Collections.Generic;
using System.Text;

namespace Platinum.Core.Abstractions.Services
{
    public interface IDateTimeService
    {
        DateTime NowUtc { get; }
    }
}
