using System;

namespace Platinum.Core.Extensions
{
    /// <summary>
    /// Represents a singleton instance of a class
    /// </summary>
    /// <typeparam name="T">Type of the singleton</typeparam>
    /// <remarks>The instance will be lazily instantiated, however do note that it will not be disposed.</remarks>
    [Obsolete("Use a dependency injection framework instead. This will be removed in a future version.")]
    public abstract class Singleton<T> where T : new()
    {
        private static readonly Lazy<T> _instance = new Lazy<T>(() => new T());

        /// <summary>
        /// Gets a singleton instance of <typeparamref name="T"/>
        /// </summary>
        public static T Instance => _instance.Value;
    }
}
