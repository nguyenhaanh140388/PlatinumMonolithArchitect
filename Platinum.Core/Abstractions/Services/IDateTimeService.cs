using System;
using System.Collections.Generic;
using System.Text;

namespace Anhny010920.Core.Abstractions.Services
{
    public interface IDateTimeService
    {
        DateTime NowUtc { get; }
    }
}
