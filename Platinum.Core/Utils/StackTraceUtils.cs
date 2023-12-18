using System;
using System.Reflection;

namespace Platinum.Core.Utils
{
    public static class StackTraceUtils
    {        /// <summary>
             /// Preserves the stack trace.
             /// </summary>
             /// <param name="exception">The exception.</param>
        public static void PreserveStackTrace(Exception exception)
        {
            MethodInfo preserveStackTrace = typeof(Exception).GetMethod(
                "InternalPreserveStackTrace",
                BindingFlags.Instance | BindingFlags.NonPublic);
            preserveStackTrace.Invoke(exception, null);
        }
    }
}
