// <copyright file="ITokenUrlEncoderService.cs" company="Anhny010920">
// Copyright (c) Anhny010920. All rights reserved.
// </copyright>

namespace Anhny010920.Core.Abstractions.Communications
{
    /// <summary>
    /// ITokenUrlEncoderService.
    /// </summary>
    public interface ITokenUrlEncoderService
    {
        /// <summary>
        /// Decodes the token.
        /// </summary>
        /// <param name="urlToken">The URL token.</param>
        /// <returns>Result.</returns>
        string DecodeToken(string urlToken);

        /// <summary>
        /// Encodes the token.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>Result.</returns>
        string EncodeToken(string token);
    }
}