// ReSharper disable UnusedMember.Global

namespace Platinum.Core.Extensions
{
    /// <summary>
    /// Provides extensions for <see cref="Random"/>.
    /// </summary>
    public static class RandomExtensions
    {
        /// <summary>
        /// Gets a random double, between <paramref name="minValue"/> and <paramref name="maxValue"/>
        /// </summary>
        public static double NextDouble(
            this Random random,
            double minValue,
            double maxValue)
        {
            return random.NextDouble() * (maxValue - minValue) + minValue;
        }

        /// <summary>
        /// Gets a random float, between <paramref name="minValue"/> and <paramref name="maxValue"/>
        /// </summary>
        public static float NextFloat(
            this Random random,
            float minValue,
            float maxValue)
        {
            return (float)random.NextDouble() * (maxValue - minValue) + minValue;
        }

        /// <summary>
        /// Generates a string with random characters. 
        /// </summary>
        /// <remarks>Use of this method for anything security related is not recommended.</remarks>
        /// <param name="random">An existing random instance to use for generating the string.</param>
        /// <param name="characters">Characters that can be chosen as part of the randomisation process.</param>
        /// <param name="length">The length of the randomised string to generate.</param>
        public static string GenerateString(this Random random, string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789", int length = 8)
        {
            if (length < 1)
            {
                throw new ArgumentException("Length must be greater than one.", nameof(length));
            }

            var stringChars = new char[length];

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = characters[random.Next(characters.Length)];
            }

            return new string(stringChars);
        }
    }
}
