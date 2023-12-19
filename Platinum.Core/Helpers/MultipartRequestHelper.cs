// <copyright file="MultipartRequestHelper.cs" company="Anhny010920">
// Copyright (c) Anhny010920. All rights reserved.
// </copyright>

namespace Platinum.Core.Helpers
{
    using System;
    using System.IO;

    /// <summary>
    /// MultipartRequestHelper.
    /// </summary>
    public static class MultipartRequestHelper
    {
        // Content-Type: multipart/form-data; boundary="----WebKitFormBoundarymx2fSWqWSd0OxQqq"
        // The spec at https://tools.ietf.org/html/rfc2046#section-5.1 states that 70 characters is a reasonable limit.

        /// <summary>
        /// Gets the boundary.
        /// </summary>
        /// <param name="contentType">Type of the content.</param>
        /// <param name="lengthLimit">The length limit.</param>
        /// <returns>Result.</returns>
        /// <exception cref="InvalidDataException">Missing content-type boundary.
        /// or
        /// Multipart boundary length limit {lengthLimit} exceeded.</exception>
        public static string GetBoundary(MediaTypeHeaderValue contentType, int lengthLimit)
        {
            string boundary = HeaderUtilities.RemoveQuotes(contentType.Boundary).Value;

            if (string.IsNullOrWhiteSpace(boundary))
            {
                throw new InvalidDataException("Missing content-type boundary.");
            }

            if (boundary.Length > lengthLimit)
            {
                throw new InvalidDataException(
                    $"Multipart boundary length limit {lengthLimit} exceeded.");
            }

            return boundary;
        }

        /// <summary>
        /// Determines whether [is multipart content type] [the specified content type].
        /// </summary>
        /// <param name="contentType">Type of the content.</param>
        /// <returns>
        ///   <c>true</c> if [is multipart content type] [the specified content type]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsMultipartContentType(string contentType)
        {
            return !string.IsNullOrEmpty(contentType)
                   && contentType.IndexOf("multipart/", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        /// <summary>
        /// Determines whether [has form data content disposition] [the specified content disposition].
        /// </summary>
        /// <param name="contentDisposition">The content disposition.</param>
        /// <returns>
        ///   <c>true</c> if [has form data content disposition] [the specified content disposition]; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasFormDataContentDisposition(ContentDispositionHeaderValue contentDisposition)
        {
            // Content-Disposition: form-data; name="key";
            return contentDisposition != null
                && contentDisposition.DispositionType.Equals("form-data")
                && string.IsNullOrEmpty(contentDisposition.FileName.Value)
                && string.IsNullOrEmpty(contentDisposition.FileNameStar.Value);
        }

        /// <summary>
        /// Determines whether [has file content disposition] [the specified content disposition].
        /// </summary>
        /// <param name="contentDisposition">The content disposition.</param>
        /// <returns>
        ///   <c>true</c> if [has file content disposition] [the specified content disposition]; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasFileContentDisposition(ContentDispositionHeaderValue contentDisposition)
        {
            // Content-Disposition: form-data; name="myfile1"; filename="Misc 002.jpg"
            return contentDisposition != null
                && contentDisposition.DispositionType.Equals("form-data")
                && (!string.IsNullOrEmpty(contentDisposition.FileName.Value)
                    || !string.IsNullOrEmpty(contentDisposition.FileNameStar.Value));
        }
    }
}
