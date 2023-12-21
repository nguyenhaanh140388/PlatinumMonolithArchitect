// <copyright file="Base32.cs" company="Platinum">
// Copyright (c) Platinum. All rights reserved.
// </copyright>

namespace Platinum.Core.Helpers
{
    using System;
    using System.Text;

    /// <summary>
    ///
    /// </summary>
    public static class Base32
    {
        /// <summary>
        /// The base32 chars.
        /// </summary>
        private static readonly string base32Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";

        /// <summary>
        /// Converts to base32.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>Result.</returns>
        /// <exception cref="ArgumentNullException">input.</exception>
        public static string ToBase32(byte[] input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            StringBuilder sb = new StringBuilder();
            for (int offset = 0; offset < input.Length;)
            {
                int numCharsToOutput = GetNextGroup(input, ref offset, out byte a, out byte b, out byte c, out byte d, out byte e, out byte f, out byte g, out byte h);

                sb.Append(numCharsToOutput >= 1 ? base32Chars[a] : '=');
                sb.Append(numCharsToOutput >= 2 ? base32Chars[b] : '=');
                sb.Append(numCharsToOutput >= 3 ? base32Chars[c] : '=');
                sb.Append(numCharsToOutput >= 4 ? base32Chars[d] : '=');
                sb.Append(numCharsToOutput >= 5 ? base32Chars[e] : '=');
                sb.Append(numCharsToOutput >= 6 ? base32Chars[f] : '=');
                sb.Append(numCharsToOutput >= 7 ? base32Chars[g] : '=');
                sb.Append(numCharsToOutput >= 8 ? base32Chars[h] : '=');
            }

            return sb.ToString();
        }

        /// <summary>
        /// Froms the base32.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>Result.</returns>
        /// <exception cref="ArgumentNullException">input.</exception>
        /// <exception cref="FormatException"></exception>
        public static byte[] FromBase32(string input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            input = input.TrimEnd('=').ToUpperInvariant();
            if (input.Length == 0)
            {
                return new byte[0];
            }

            var output = new byte[input.Length * 5 / 8];
            var bitIndex = 0;
            var inputIndex = 0;
            var outputBits = 0;
            var outputIndex = 0;
            while (outputIndex < output.Length)
            {
                var byteIndex = base32Chars.IndexOf(input[inputIndex]);
                if (byteIndex < 0)
                {
                    throw new FormatException();
                }

                var bits = Math.Min(5 - bitIndex, 8 - outputBits);
                output[outputIndex] <<= bits;
                output[outputIndex] |= (byte)(byteIndex >> 5 - (bitIndex + bits));

                bitIndex += bits;
                if (bitIndex >= 5)
                {
                    inputIndex++;
                    bitIndex = 0;
                }

                outputBits += bits;
                if (outputBits >= 8)
                {
                    outputIndex++;
                    outputBits = 0;
                }
            }

            return output;
        }

        // returns the number of bytes that were output

        /// <summary>
        /// Gets the next group.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <param name="c">The c.</param>
        /// <param name="d">The d.</param>
        /// <param name="e">The e.</param>
        /// <param name="f">The f.</param>
        /// <param name="g">The g.</param>
        /// <param name="h">The h.</param>
        /// <returns>Result.</returns>
        private static int GetNextGroup(byte[] input, ref int offset, out byte a, out byte b, out byte c, out byte d, out byte e, out byte f, out byte g, out byte h)
        {
            uint b1, b2, b3, b4, b5;

            int retVal;
            switch (offset - input.Length)
            {
                case 1: retVal = 2; break;
                case 2: retVal = 4; break;
                case 3: retVal = 5; break;
                case 4: retVal = 7; break;
                default: retVal = 8; break;
            }

            b1 = offset < input.Length ? input[offset++] : 0U;
            b2 = offset < input.Length ? input[offset++] : 0U;
            b3 = offset < input.Length ? input[offset++] : 0U;
            b4 = offset < input.Length ? input[offset++] : 0U;
            b5 = offset < input.Length ? input[offset++] : 0U;

            a = (byte)(b1 >> 3);
            b = (byte)((b1 & 0x07) << 2 | b2 >> 6);
            c = (byte)(b2 >> 1 & 0x1f);
            d = (byte)((b2 & 0x01) << 4 | b3 >> 4);
            e = (byte)((b3 & 0x0f) << 1 | b4 >> 7);
            f = (byte)(b4 >> 2 & 0x1f);
            g = (byte)((b4 & 0x3) << 3 | b5 >> 5);
            h = (byte)(b5 & 0x1f);

            return retVal;
        }
    }
}
