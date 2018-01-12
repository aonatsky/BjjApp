using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using TRNMNT.Core.Enum;
using TRNMNT.Core.Model;
using TRNMNT.Core.Services.Interface;

namespace TRNMNT.Core.Services.Impl
{
    public abstract class BaseFileProcessiongService<T> : IFileProcessiongService<T>
    {
        #region Dependencies

        private readonly IHostingEnvironment _env;

        #endregion

        #region .ctor

        protected BaseFileProcessiongService(IHostingEnvironment env)
        {
            _env = env;
        }

        #endregion

        #region Public Methods

        public async Task<FileProcessResult> ProcessFileAsync(IFormFile file, T processingOptions)
        {
            if (file == null)
            {
                return new FileProcessResult(FileProcessResultEnum.FileIsNull);
            }
            if (file.Length == 0)
            {
                return new FileProcessResult(FileProcessResultEnum.FileIsEmpty);
            }

            var internalProcessResult = await ProcessInternalAsync(file.OpenReadStream(), processingOptions);
            return internalProcessResult;
        }

        #endregion

        #region Protected Abstract Methods

        protected abstract Task<FileProcessResult> ProcessInternalAsync(Stream stream, T processingOptions);

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
