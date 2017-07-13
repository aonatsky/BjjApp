using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace TRNMNT.Core.Services
{
    public interface IFileService
    {
        Task SaveFileAsync(string path, Stream stream);
        Task<bool> SaveFileAsync(string path);
        Task<bool> IsFileExistAsync(string path);
        Task SaveImageAsync(string path, Stream stream);

    }
}
