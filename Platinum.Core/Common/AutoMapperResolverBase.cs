// Copyright (c) Anhny010920. All rights reserved.
// </copyright>

using AutoMapper;

namespace Platinum.Core.Common
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="S"></typeparam>
    /// <typeparam name="D"></typeparam>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="IValueResolver{S, D, T}" />
    public abstract class AutoMapperResolverBase<S, D, T> : IValueResolver<S, D, T>
    {
        /// <summary>
        /// The resolve action.
        /// </summary>
        protected Func<S, D, T> ResolveAction;

        /// <summary>
        /// Implementors use source object to provide a destination object.
        /// </summary>
        /// <param name="source">Source object.</param>
        /// <param name="destination">Destination object, if exists.</param>
        /// <param name="destMember">Destination member.</param>
        /// <param name="context">The context of the mapping.</param>
        /// <returns>
        /// Result, typically build from the source resolution result.
        /// </returns>
        T IValueResolver<S, D, T>.Resolve(S source, D destination, T destMember, ResolutionContext context)
        {
            return ResolveAction(source, destination);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoMapperResolverBase{S, D, T}" /> class.
        /// </summary>
        public AutoMapperResolverBase()
        {
            ResolveAction = ResolveCalculate;
        }

        /// <summary>
        /// Resolves the calculate.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        /// <returns>Result.</returns>
        public abstract T ResolveCalculate(S source, D destination);
    }
}
