using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using TRNMNT.Web.Core.Enum;
using TRNMNT.Web.Core.Model;

namespace TRNMNT.Web.Core.Services.impl
{
    public abstract class BaseFileService
    {
        protected abstract FileProcessResult ProcessInternal(Stream stream);

        protected abstract string GetFileUploadPath(string rootPath);


        private IHostingEnvironment env;


        public BaseFileService(IHostingEnvironment env)
        {
            this.env = env;
        }

        protected string GetWebRootPath()
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

            var internalProcessResult = ProcessInternal(file.OpenReadStream());
            return internalProcessResult;
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
