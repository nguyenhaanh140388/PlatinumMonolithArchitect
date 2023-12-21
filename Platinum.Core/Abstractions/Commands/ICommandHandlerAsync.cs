// <copyright file="ICommandHandlerAsync.cs" company="Platinum">
// Copyright (c) Platinum. All rights reserved.
// </copyright>

namespace Platinum.Core.Abstractions.Commands
{
    /// <summary>
    /// ICommandHandlerAsync.
    /// </summary>
    /// <typeparam name="TPayload">The type of the payload.</typeparam>
    /// <seealso cref="ICommandRoot" />
    /// <seealso cref="ICommandRoot" />
    public partial interface ICommandHandlerAsync<in TPayload> : ICommandRoot
    {
        /// <summary>
        /// Handles the asynchronous.
        /// </summary>
        /// <param name="payload">The payload.</param>
        /// <returns>Result.</returns>
        Task HandleAsync(TPayload payload);
    }

    /// <summary>
    /// ICommandHandlerAsync.
    /// </summary>
    /// <typeparam name="TPayload">The type of the payload.</typeparam>
    /// <typeparam name="TResponse">The type of the response.</typeparam>
    /// <seealso cref="ICommandRoot" />
    /// <seealso cref="ICommandRoot" />
    public interface ICommandHandlerAsync<in TPayload, TResponse> : ICommandRoot
    {
        /// <summary>
        /// Handles the asynchronous.
        /// </summary>
        /// <param name="payload">The payload.</param>
        /// <returns>
        /// A <see cref="Task" /> representing the asynchronous operation.
        /// </returns>
        Task<TResponse> HandleAsync(TPayload payload);
    }
}
