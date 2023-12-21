// <copyright file="LoginStatus.cs" company="Platinum">
// Copyright (c) Platinum. All rights reserved.
// </copyright>

namespace Platinum.Core.Enums
{
    /// <summary>
    /// LoginStatus.
    /// </summary>
    public enum LoginStatus
    {
        /// <summary>
        /// The email confirmed.
        /// </summary>
        EmailConfirmed,

        /// <summary>
        /// The wrong password.
        /// </summary>
        WrongPassword,

        /// <summary>
        /// The is locked out.
        /// </summary>
        IsLockedOut,

        /// <summary>
        /// The no exist.
        /// </summary>
        NoExist,

        /// <summary>
        /// The sign in failed.
        /// </summary>
        SignInFailed,

        /// <summary>
        /// The login succeeded.
        /// </summary>
        Succeeded,

        /// <summary>
        /// The sign in two factor
        /// </summary>
        SignInTwoFactor,
    }
}
