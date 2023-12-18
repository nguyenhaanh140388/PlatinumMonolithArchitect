// <copyright file="IQueryHandler.cs" company="Anhny010920">
// Copyright (c) Anhny010920. All rights reserved.
// </copyright>

namespace Anhny010920.Core.Abstractions.Queries
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TResponse">The type of the response.</typeparam>
    /// <seealso cref="IQueryRoot" />
    public interface IQueryHandler<out TResponse> : IQueryRoot
    {
        /// <summary>
        /// Fetches this instance.
        /// </summary>
        /// <returns>Result.</returns>
        TResponse Fetch();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TPayload">The type of the payload.</typeparam>
    /// <typeparam name="TResponse">The type of the response.</typeparam>
    /// <seealso cref="IQueryRoot" />
    public interface IQueryHandler<in TPayload, out TResponse> : ICommandRoot
    {
        /// <summary>
        /// Handles the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        TResponse Fetch(TPayload command);
    }
}
