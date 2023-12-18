// <copyright file="CommandHandlerNotFoundException.cs" company="Anhny010920">
// Copyright (c) Anhny010920. All rights reserved.
// </copyright>

using System;

namespace Platinum.Core.Exceptions
{
    public class CommandHandlerNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandHandlerNotFoundException"/> class.
        /// </summary>
        public CommandHandlerNotFoundException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandHandlerNotFoundException"/> class.
        /// </summary>
        /// <param name="type"></param>
        public CommandHandlerNotFoundException(Type type)
            : base()
        {
        }
    }
}
