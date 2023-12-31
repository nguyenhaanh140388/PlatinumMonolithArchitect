﻿// <copyright file="ICommandHandler.cs" company="Platinum">
// Copyright (c) Platinum. All rights reserved.
// </copyright>

namespace Platinum.Core.Abstractions.Commands
{
    /// <summary>
    ///   <br />
    /// </summary>
    /// <typeparam name="TPayload">The type of the payload.</typeparam>
    /// <seealso cref="ICommandRoot" />
    public interface ICommandHandler<TPayload> : ICommandRoot
    {
        /// <summary>
        /// Handles this instance.
        /// </summary>
        /// <param name="payload">The payload.</param>
        void Handle(TPayload payload);
    }

    /// <summary>
    ///   <br />
    /// </summary>
    /// <typeparam name="TPayload">The type of the payload.</typeparam>
    /// <typeparam name="TResponse">The type of the response.</typeparam>
    /// <seealso cref="ICommandRoot" />
    public interface ICommandHandler<in TPayload, out TResponse> : ICommandRoot
    {
        /// <summary>
        /// Handles the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        TResponse Handle(TPayload command);
    }
}
