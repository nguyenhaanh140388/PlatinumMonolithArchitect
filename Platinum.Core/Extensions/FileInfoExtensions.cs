namespace Platinum.Core.Extensions
{
    /// <summary>
    /// Provides extensions for the <see cref="FileInfo" /> class.
    /// </summary>
    public static class FileInfoExtensions
    {
        /// <summary>
        /// Attempts to rename a file on the filesystem. 
        /// </summary>
        /// <param name="fileInfo">The file which should be renamed.</param>
        /// <param name="newName">The new name (including file extension) of the file.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="fileInfo"/> or <paramref name="newName"/> are null.</exception>
        public static void Rename(this FileInfo fileInfo, string newName)
        {
            if (fileInfo == null)
            {
                throw new ArgumentNullException(nameof(fileInfo));
            }

            if (string.IsNullOrEmpty(newName))
            {
                throw new ArgumentNullException(nameof(newName));
            }

            fileInfo.MoveTo(Path.Combine(fileInfo.Directory.FullName, newName));
        }
    }
}
