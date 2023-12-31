﻿namespace Platinum.Core.Extensions
{
    public static class NullbaleExtensions
    {
        public static bool HasNoValue<T>(this T? source) where T : struct => !source.HasValue;
    }
}