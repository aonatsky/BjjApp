using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using TRNMNT.Core.Services.Interface;

namespace TRNMNT.Core.Services.Impl
{
    public class LocalFileService : IFileService
    {
        #region Properties

        private readonly string _rootPath;

        #endregion

        #region .ctor

        public LocalFileService(IHostingEnvironment env)
        {
            _rootPath = env.WebRootPath;
        }

        #endregion

        #region Public Methods

        public Task<bool> IsFileExistAsync(string path)
        {
            throw new NotImplementedException();
        }

        public async Task SaveFileAsync(string relativePath, Stream stream)
        {
            var path = Path.Combine(_rootPath, relativePath);
            CheckPath(path);
            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                await stream.CopyToAsync(fileStream);
            }
        }

        public Task<bool> SaveFileAsync(string path)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SaveFileAsync(string name, string path)
        {
            throw new NotImplementedException();
        }

        public async Task SaveImageAsync(string relativePath, Stream stream, string fileName)
        {
            var path = Path.Combine(_rootPath, relativePath);
            CheckPath(path);
            var img = Image.FromStream(stream, true, true);
            img.Save(path, ImageFormat.Jpeg);
        }

        #endregion

        #region Private Methods

        private void CheckPath(string path)
        {
            var pathInfo = new FileInfo(path);
            pathInfo.Directory.Create();
        }

        #endregion
    }
}
