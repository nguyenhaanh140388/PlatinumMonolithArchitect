using System;
using System.Collections.Generic;
using System.Text;

namespace Platinum.Core.Utils
{
    public static class PasswordHashUtils
    {      /// <summary>
           /// Verifies the password hash.
           /// </summary>
           /// <param name="password">The password.</param>
           /// <param name="storedHash">The stored hash.</param>
           /// <param name="storedSalt">The stored salt.</param>
           /// <returns>Result.</returns>
           /// <exception cref="ArgumentNullException">password.</exception>
           /// <exception cref="ArgumentException">Value cannot be empty or whitespace only string. - password
           /// or
           /// Invalid length of password hash (64 bytes expected). - passwordHash
           /// or
           /// Invalid length of password salt (128 bytes expected). - passwordHash.</exception>
        public static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            }

            if (storedHash.Length != 64)
            {
                throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            }

            if (storedSalt.Length != 128)
            {
                throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");
            }

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Creates the password hash.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <param name="passwordHash">The password hash.</param>
        /// <param name="passwordSalt">The password salt.</param>
        /// <returns>Result.</returns>
        /// <exception cref="ArgumentNullException">password.</exception>
        /// <exception cref="ArgumentException">Value cannot be empty or whitespace only string. - password.</exception>
        public static bool CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Value cannot be empty or whitespace only string.", nameof(password));
            }

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }

            return true;
        }
    }
}
