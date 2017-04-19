using System.IO;
using Microsoft.AspNetCore.Hosting;
using TRNMNT.Core.Enum;
using Microsoft.AspNetCore.Http;
using TRNMNT.Core.Model;

namespace TRNMNT.Core.Services
{
    public abstract class FileService
    {
        protected abstract FileProcessResult PostUploadProcess(Stream stream);
        
        protected abstract string GetFilePath(string rootPath);
        

        private IHostingEnvironment env;
        public FileService(IHostingEnvironment env)
        {
            this.env = env;
        }

        public string GetWebRootPath()
        {
            return this.env.WebRootPath;
        }


        public FileProcessResult ProcessFile(IFormFile file)
        {
            if (file == null)
            {
                return new FileProcessResult(FileProcessResultEnum.FileIsNull);
            }
            if (file.Length == 0)
            {
                return new FileProcessResult(FileProcessResultEnum.FileIsEmpty);
            }

            var postUploadProcessResult = PostUploadProcess(file.OpenReadStream());
            if (postUploadProcessResult.Result == FileProcessResultEnum.Success || postUploadProcessResult.Result == FileProcessResultEnum.SuccessWithErrors)
            {
                SaveFile(file);
            }

            return postUploadProcessResult;
        }

        private void SaveFile(IFormFile file)
        {
            using (var fileStream = new FileStream(GetFilePath(GetWebRootPath()), FileMode.Create))
            {
                file.CopyTo(fileStream);
            }
        }
    }
}

