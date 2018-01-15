using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TRNMNT.Core.Model;

namespace TRNMNT.Core.Services.Interface
{
    public interface IFileProcessiongService<in T>
    {
        Task<FileProcessResult> ProcessFileAsync(IFormFile file, T options);
    }
}