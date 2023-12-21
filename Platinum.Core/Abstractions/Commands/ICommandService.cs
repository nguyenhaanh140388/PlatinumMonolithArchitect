// <copyright file="ICommandService.cs" company="Platinum">
// Copyright (c) Platinum. All rights reserved.
// </copyright>

namespace Platinum.Core.Abstractions.Commands
{
    /// <summary>
    /// ICommandFactory.
    /// </summary>
    public interface ICommandService
    {
        /// <summary>
        /// Handles the specified payload.
        /// </summary>
        /// <typeparam name="TPayload">The type of the payload.</typeparam>
        /// <param name="payload">The payload.</param>
        void Handle<TPayload>(TPayload payload);

        /// <summary>
        /// Handles the specified command.
        /// </summary>
        /// <typeparam name="TPayload">The type of the payload.</typeparam>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="payload">The payload.</param>
        /// <returns>Result.</returns>
        TResponse Handle<TPayload, TResponse>(TPayload payload);

        /// <summary>
        /// Handles the asynchronous.
        /// </summary>
        /// <typeparam name="TPayload">The type of the payload.</typeparam>
        /// <param name="payload">The payload.</param>
        /// <returns>
        /// A <see cref="Task" /> representing the asynchronous operation.
        /// </returns>
        Task HandleAsync<TPayload>(TPayload payload);

        /// <summary>
        /// Invokes the asynchronous.
        /// </summary>
        /// <typeparam name="TPayload">The type of the payload.</typeparam>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="payload">The payload.</param>
        /// <returns>
        /// A <see cref="Task" /> representing the asynchronous operation.
        /// </returns>
        Task<TResponse> HandleAsync<TPayload, TResponse>(TPayload payload);

        /// <summary>
        /// Fetches this instance.
        /// </summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <returns>Result.</returns>
        TResponse Fetch<TResponse>();

        /// <summary>
        /// Handles the specified command.
        /// </summary>
        /// <typeparam name="TPayload">The type of the payload.</typeparam>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="payload">The payload.</param>
        /// <returns>Result.</returns>
        TResponse Fetch<TPayload, TResponse>(TPayload payload);

        /// <summary>
        /// Handles the asynchronous.
        /// </summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <returns>
        /// A <see cref="Task" /> representing the asynchronous operation.
        /// </returns>
        Task<TResponse> FetchAsync<TResponse>();

        /// <summary>
        /// Invokes the asynchronous.
        /// </summary>
        /// <typeparam name="TPayload">The type of the payload.</typeparam>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="payload">The payload.</param>
        /// <returns>
        /// A <see cref="Task" /> representing the asynchronous operation.
        /// </returns>
        Task<TResponse> FetchAsync<TPayload, TResponse>(TPayload payload);
    }
}
