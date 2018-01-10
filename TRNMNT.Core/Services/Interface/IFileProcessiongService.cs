using Microsoft.AspNetCore.Http;
using TRNMNT.Core.Model;

namespace TRNMNT.Core.Services.Interface
{
    public interface IFileProcessiongService
    {
        FileProcessResult ProcessFile(IFormFile file);
    }
}