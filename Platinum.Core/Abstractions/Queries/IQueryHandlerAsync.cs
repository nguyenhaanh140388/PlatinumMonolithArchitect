// <copyright file="IQueryHandlerAsync.cs" company="Anhny010920">
// Copyright (c) Anhny010920. All rights reserved.
// </copyright>

using System.Threading.Tasks;

namespace Anhny010920.Core.Abstractions.Queries
{
    /// <summary>
    /// IQueryHandlerAsync.
    /// </summary>
    /// <typeparam name="TResponse">The type of the response.</typeparam>
    /// <seealso cref="IQueryRoot" />
    /// <seealso cref="IQueryRoot" />
    public interface IQueryHandlerAsync<TResponse> : IQueryRoot
    {
        /// <summary>
        /// Fetches the asynchronous.
        /// </summary>
        /// <returns>
        /// A <see cref="Task" /> representing the asynchronous operation.
        /// </returns>
        Task<TResponse> FetchAsync();
    }

    /// <summary>
    /// IQueryHandlerAsync.
    /// </summary>
    /// <typeparam name="TPayload">The type of the payload.</typeparam>
    /// <typeparam name="TResponse">The type of the response.</typeparam>
    /// <seealso cref="IQueryRoot" />
    /// <seealso cref="IQueryRoot" />
    public interface IQueryHandlerAsync<in TPayload, TResponse> : ICommandRoot
    {
        /// <summary>
        /// Handles the specified command.
        /// </summary>
        /// <param name="payload">The payload.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        Task<TResponse> FetchAsync(TPayload payload);
    }
}
