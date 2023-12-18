namespace Platinum.Core.Extensions
{
    /// <summary>
    /// Interface for objects which may implement a completion method.
    /// </summary>
    public interface IOnComplete<in T>
    {
        /// <summary>
        /// Called when an action is completed.
        /// </summary>
        /// <param name="data">Data payload to pass.</param>
        void OnComplete(T data);
    }
}