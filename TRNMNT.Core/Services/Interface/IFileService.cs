using System.IO;
using System.Threading.Tasks;

namespace TRNMNT.Core.Services.Interface
{
    public interface IFileService
    {
        Task SaveFileAsync(string path, Stream stream);
        Task<bool> SaveFileAsync(string path);
        Task<bool> IsFileExistAsync(string path);
        Task SaveImageAsync(string path, Stream stream, string fileName);

    }
}
