using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using TRNMNT.Core.Enum;
using TRNMNT.Core.Model;

namespace TRNMNT.Core.Services.Impl
{
    public abstract class BaseFileService
    {
        #region Dependencies

        private readonly IHostingEnvironment _env;

        #endregion

        #region .ctor

        protected BaseFileService(IHostingEnvironment env)
        {
            _env = env;
        }

        #endregion

        #region Public Methods

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

        #endregion

        #region Protected Abstract Methods

        protected abstract FileProcessResult ProcessInternal(Stream stream);

        protected abstract string GetFileUploadPath(string rootPath);

        #endregion

        #region Protected Methods

        protected string GetWebRootPath()
        {
            return _env.WebRootPath;
        }

        #endregion

        #region Private Methods

        private void SaveFile(IFormFile file)
        {
            using (var fileStream = new FileStream(GetFileUploadPath(GetWebRootPath()), FileMode.Create))
            {
                file.CopyTo(fileStream);
            }
        }

        #endregion
    }
}
