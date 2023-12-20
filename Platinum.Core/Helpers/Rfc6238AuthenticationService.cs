// <copyright file="Rfc6238AuthenticationService.cs" company="Anhny010920">
// Copyright (c) Anhny010920. All rights reserved.
// </copyright>

namespace Platinum.Core.Helpers
{
    using System;
    using System.Diagnostics;
    using System.Net;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// Rfc6238AuthenticationService.
    /// </summary>
    public static class Rfc6238AuthenticationService
    {
        /// <summary>
        /// The unix epoch.
        /// </summary>
        private static readonly DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// The timestep.
        /// </summary>
        private static readonly TimeSpan timestep = TimeSpan.FromMinutes(3);

        /// <summary>
        /// The encoding.
        /// </summary>
        private static readonly Encoding encoding = new UTF8Encoding(false, true);

        /// <summary>
        /// The RNG.
        /// </summary>
        private static readonly RandomNumberGenerator rng = RandomNumberGenerator.Create();

        // Generates a new 80-bit security token

        /// <summary>
        /// Generates the random key.
        /// </summary>
        /// <returns>Result.</returns>
        public static byte[] GenerateRandomKey()
        {
            byte[] bytes = new byte[20];
            rng.GetBytes(bytes);
            return bytes;
        }

        /// <summary>
        /// Computes the totp.
        /// </summary>
        /// <param name="hashAlgorithm">The hash algorithm.</param>
        /// <param name="timestepNumber">The timestep number.</param>
        /// <param name="modifier">The modifier.</param>
        /// <returns>Result.</returns>
        public static int ComputeTotp(HashAlgorithm hashAlgorithm, ulong timestepNumber, string modifier)
        {
            // # of 0's = length of pin
            const int Mod = 1000000;

            // See https://tools.ietf.org/html/rfc4226
            // We can add an optional modifier
            var timestepAsBytes = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((long)timestepNumber));
            var hash = hashAlgorithm.ComputeHash(ApplyModifier(timestepAsBytes, modifier));

            // Generate DT string
            var offset = hash[hash.Length - 1] & 0xf;
            Debug.Assert(offset + 4 < hash.Length);
            var binaryCode = (hash[offset] & 0x7f) << 24
                             | (hash[offset + 1] & 0xff) << 16
                             | (hash[offset + 2] & 0xff) << 8
                             | hash[offset + 3] & 0xff;

            return binaryCode % Mod;
        }

        /// <summary>
        /// Applies the modifier.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="modifier">The modifier.</param>
        /// <returns>Result.</returns>
        private static byte[] ApplyModifier(byte[] input, string modifier)
        {
            if (string.IsNullOrEmpty(modifier))
            {
                return input;
            }

            var modifierBytes = encoding.GetBytes(modifier);
            var combined = new byte[checked(input.Length + modifierBytes.Length)];
            Buffer.BlockCopy(input, 0, combined, 0, input.Length);
            Buffer.BlockCopy(modifierBytes, 0, combined, input.Length, modifierBytes.Length);
            return combined;
        }

        // More info: https://tools.ietf.org/html/rfc6238#section-4

        /// <summary>
        /// Gets the current time step number.
        /// </summary>
        /// <returns>Result.</returns>
        private static ulong GetCurrentTimeStepNumber()
        {
            var delta = DateTime.UtcNow - unixEpoch;
            return (ulong)(delta.Ticks / timestep.Ticks);
        }

        /// <summary>
        /// Generates the code.
        /// </summary>
        /// <param name="securityToken">The security token.</param>
        /// <param name="modifier">The modifier.</param>
        /// <returns>Result.</returns>
        /// <exception cref="ArgumentNullException">securityToken.</exception>
        public static int GenerateCode(byte[] securityToken, string modifier = null)
        {
            if (securityToken == null)
            {
                throw new ArgumentNullException(nameof(securityToken));
            }

            // Allow a variance of no greater than 9 minutes in either direction
            var currentTimeStep = GetCurrentTimeStepNumber();
            using (var hashAlgorithm = new HMACSHA1(securityToken))
            {
                return ComputeTotp(hashAlgorithm, currentTimeStep, modifier);
            }
        }

        /// <summary>
        /// Validates the code.
        /// </summary>
        /// <param name="securityToken">The security token.</param>
        /// <param name="code">The code.</param>
        /// <param name="modifier">The modifier.</param>
        /// <returns>Result.</returns>
        /// <exception cref="ArgumentNullException">securityToken.</exception>
        public static bool ValidateCode(byte[] securityToken, int code, string modifier = null)
        {
            if (securityToken == null)
            {
                throw new ArgumentNullException(nameof(securityToken));
            }

            // Allow a variance of no greater than 9 minutes in either direction
            var currentTimeStep = GetCurrentTimeStepNumber();
            using (var hashAlgorithm = new HMACSHA1(securityToken))
            {
                for (var i = -2; i <= 2; i++)
                {
                    var computedTotp = ComputeTotp(hashAlgorithm, (ulong)((long)currentTimeStep + i), modifier);
                    if (computedTotp == code)
                    {
                        return true;
                    }
                }
            }

            // No match
            return false;
        }
    }
}
