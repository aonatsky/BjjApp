using System.IO;
using Microsoft.AspNetCore.Hosting;
using TRNMNT.Core.Enum;
using Microsoft.AspNetCore.Http;

namespace TRNMNT.Core.Services
{
    public abstract class FileService
    {
        protected abstract FileProcessResultEnum PostUploadProcess(Stream stream);
        
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


        public FileProcessResultEnum ProcessFile(IFormFile file)
        {
            if (file == null)
            {
                return FileProcessResultEnum.FileIsNull;
            }
            if (file.Length == 0)
            {
                return FileProcessResultEnum.FileIsEmpty;
            }

            var postUploadProcessResult = PostUploadProcess(file.OpenReadStream());
            if (postUploadProcessResult == FileProcessResultEnum.Success)
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

