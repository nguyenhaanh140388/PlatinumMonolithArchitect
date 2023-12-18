// <copyright file="CommandService.cs" company="Anhny010920">
// Copyright (c) Anhny010920. All rights reserved.
// </copyright>

using Anhny010920.Core.Abstractions.Commands;
using Anhny010920.Core.Abstractions.Queries;
using Anhny010920.Core.Exceptions;
using Autofac;
using System.Threading.Tasks;

namespace Platinum.Core.Commands
{
    /// <summary>
    /// CommandService.
    /// </summary>
    /// <seealso cref="ICommandService" />
    /// <seealso cref="ICommandService" />
    /// <seealso cref="Anhny010920.Interfaces.Commands.ICommandFactory" />
    public class CommandService : ICommandService
    {
        /// <summary>
        /// The component context.
        /// </summary>
        private readonly IComponentContext componentContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandService" /> class.
        /// </summary>
        /// <param name="componentContext">The component context.</param>
        public CommandService(IComponentContext componentContext)
        {
            this.componentContext = componentContext;
        }

        /// <summary>
        /// Handles the specified command.
        /// </summary>
        /// <typeparam name="TPayload">The type of the payload.</typeparam>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="payload">The payload.</param>
        /// <returns>
        /// TResponse.
        /// </returns>
        /// <exception cref="CommandHandlerNotFoundException"></exception>
        /// <exception cref="CommandHandlerNotFoundException"></exception>
        public TResponse Fetch<TPayload, TResponse>(TPayload payload)
        {
            var commandHandler = componentContext.ResolveOptional<IQueryHandler<TPayload, TResponse>>();
            if (commandHandler == null)
            {
                throw new CommandHandlerNotFoundException(typeof(TPayload));
            }

            return commandHandler.Fetch(payload);
        }

        /// <summary>
        /// Fetches this instance.
        /// </summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <returns>Result.</returns>
        /// <exception cref="CommandHandlerNotFoundException"></exception>
        /// <exception cref="CommandHandlerNotFoundException"></exception>
        public TResponse Fetch<TResponse>()
        {
            var commandHandler = componentContext.ResolveOptional<IQueryHandler<TResponse>>();
            if (commandHandler == null)
            {
                throw new CommandHandlerNotFoundException();
            }

            return commandHandler.Fetch();
        }

        /// <summary>
        /// Handles the asynchronous.
        /// </summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <returns>
        /// A <see cref="T:System.Threading.Tasks.Task" /> representing the asynchronous operation.
        /// </returns>
        /// <exception cref="CommandHandlerNotFoundException"></exception>
        /// <exception cref="CommandHandlerNotFoundException"></exception>
        public Task<TResponse> FetchAsync<TResponse>()
        {
            var commandHandler = componentContext.ResolveOptional<IQueryHandlerAsync<TResponse>>();
            if (commandHandler == null)
            {
                throw new CommandHandlerNotFoundException();
            }

            return commandHandler.FetchAsync();
        }

        /// <summary>
        /// Invokes the asynchronous.
        /// </summary>
        /// <typeparam name="TPayload">The type of the payload.</typeparam>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="payload">The payload.</param>
        /// <returns>
        /// A <see cref="T:System.Threading.Tasks.Task" /> representing the asynchronous operation.
        /// </returns>
        /// <exception cref="CommandHandlerNotFoundException"></exception>
        /// <exception cref="CommandHandlerNotFoundException"></exception>
        public Task<TResponse> FetchAsync<TPayload, TResponse>(TPayload payload)
        {
            var commandHandler = componentContext.ResolveOptional<IQueryHandlerAsync<TPayload, TResponse>>();
            if (commandHandler == null)
            {
                throw new CommandHandlerNotFoundException(typeof(TPayload));
            }

            return commandHandler.FetchAsync(payload);
        }

        /// <summary>
        /// Handles the specified payload.
        /// </summary>
        /// <typeparam name="TPayload">The type of the payload.</typeparam>
        /// <param name="payload">The payload.</param>
        /// <exception cref="CommandHandlerNotFoundException"></exception>
        /// <exception cref="CommandHandlerNotFoundException"></exception>
        public void Handle<TPayload>(TPayload payload)
        {
            var commandHandler = componentContext.ResolveOptional<ICommandHandler<TPayload>>();
            if (commandHandler == null)
            {
                throw new CommandHandlerNotFoundException(typeof(TPayload));
            }

            commandHandler.Handle(payload);
        }

        /// <summary>
        /// Handles the specified command.
        /// </summary>
        /// <typeparam name="TPayload">The type of the payload.</typeparam>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="payload">The payload.</param>
        /// <returns>Result.</returns>
        /// <exception cref="CommandHandlerNotFoundException"></exception>
        /// <exception cref="CommandHandlerNotFoundException"></exception>
        public TResponse Handle<TPayload, TResponse>(TPayload payload)
        {
            var commandHandler = componentContext.ResolveOptional<ICommandHandler<TPayload, TResponse>>();
            if (commandHandler == null)
            {
                throw new CommandHandlerNotFoundException(typeof(TPayload));
            }

            return commandHandler.Handle(payload);
        }

        /// <summary>
        /// Handles the asynchronous.
        /// </summary>
        /// <typeparam name="TPayload">The type of the payload.</typeparam>
        /// <param name="payload">The payload.</param>
        /// <returns>
        /// A <see cref="T:System.Threading.Tasks.Task" /> representing the asynchronous operation.
        /// </returns>
        /// <exception cref="CommandHandlerNotFoundException"></exception>
        /// <exception cref="CommandHandlerNotFoundException"></exception>
        public Task HandleAsync<TPayload>(TPayload payload)
        {
            var commandHandler = componentContext.ResolveOptional<ICommandHandlerAsync<TPayload>>();
            if (commandHandler == null)
            {
                throw new CommandHandlerNotFoundException(typeof(TPayload));
            }

            return commandHandler.HandleAsync(payload);
        }

        /// <summary>
        /// Invokes the asynchronous.
        /// </summary>
        /// <typeparam name="TPayload">The type of the payload.</typeparam>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="payload">The payload.</param>
        /// <returns>
        /// A <see cref="T:System.Threading.Tasks.Task" /> representing the asynchronous operation.
        /// </returns>
        /// <exception cref="CommandHandlerNotFoundException"></exception>
        /// <exception cref="CommandHandlerNotFoundException"></exception>
        public Task<TResponse> HandleAsync<TPayload, TResponse>(TPayload payload)
        {
            var commandHandler = componentContext.ResolveOptional<ICommandHandlerAsync<TPayload, TResponse>>();
            if (commandHandler == null)
            {
                throw new CommandHandlerNotFoundException(typeof(TPayload));
            }

            return commandHandler.HandleAsync(payload);
        }
    }
}
