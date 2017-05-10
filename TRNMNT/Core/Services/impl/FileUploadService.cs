using System.IO;
using Microsoft.AspNetCore.Hosting;
using TRNMNT.Core.Enum;
using Microsoft.AspNetCore.Http;
using TRNMNT.Core.Model;
using System.Threading.Tasks;

namespace TRNMNT.Core.Services
{
    public abstract class FileUploadService
    {
        protected abstract FileProcessResult PostUploadProcess(Stream stream);
        
        protected abstract string GetFileUploadPath(string rootPath);
        

        private IHostingEnvironment env;
        public FileUploadService(IHostingEnvironment env)
        {
            this.env = env;
        }

        public string GetWebRootPath()
        {
            return this.env.WebRootPath;
        }


        public Task<FileProcessResult> ProcessFileAsync(IFormFile file)
        {
            if (file == null)
            {
                return Task.FromResult(new FileProcessResult(FileProcessResultEnum.FileIsNull));
            }
            if (file.Length == 0)
            {
                return Task.FromResult(new FileProcessResult(FileProcessResultEnum.FileIsEmpty));
            }

            var postUploadProcessResult = PostUploadProcess(file.OpenReadStream());
            if (postUploadProcessResult.Code == FileProcessResultEnum.Success || postUploadProcessResult.Code == FileProcessResultEnum.SuccessWithErrors)
            {
                SaveFile(file);
            }

            return Task.FromResult(postUploadProcessResult);
        }

        private void SaveFile(IFormFile file)
        {
            using (var fileStream = new FileStream(GetFileUploadPath(GetWebRootPath()), FileMode.Create))
            {
                file.CopyTo(fileStream);
            }
        }
    }
}

