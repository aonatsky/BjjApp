using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TRNMNT.Core.Services
{
    interface IFileService
    {
        Task<bool> SaveFileAsync(string name, string path);
        Task<bool> SaveFileAsync(string path);
        Task<bool> IsFileExistAsync(string path);

    }
}
