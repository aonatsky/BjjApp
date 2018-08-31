using System.IO;
using System.Threading.Tasks;

namespace TRNMNT.Core.Services.Interface
{
    public interface IFileService
    {
        /// <summary>
        /// Saves the file asynchronous.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        Task SaveFileAsync(string path, Stream stream);

        /// <summary>
        /// Saves the file asynchronous.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        Task<bool> SaveFileAsync(string path);

        /// <summary>
        /// Determines whether [is file exist asynchronous] [the specified path].
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        Task<bool> IsFileExistAsync(string path);
    }
}
