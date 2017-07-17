using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace TRNMNT.Core.Services
{
    public class LocalFileService : IFileService
    {
        private string rootPath;

        public LocalFileService(IHostingEnvironment env)
        {
            rootPath = env.WebRootPath;
        }
        public Task<bool> IsFileExistAsync(string path)
        {
            throw new NotImplementedException();
        }

        public async Task SaveFileAsync(string path, Stream stream)
        {
            using (var fileStream = new FileStream(Path.Combine(rootPath, path), FileMode.Create))
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

        public async Task SaveImageAsync(string path, Stream stream, string fileName)
        {
            CheckPath(path);
            Image img = Image.FromStream(stream, true, true);
            img.Save(Path.Combine(rootPath,path), ImageFormat.Jpeg);
        }

        private void CheckPath(string path)
        {
            var pathInfo = new FileInfo(path);
            pathInfo.Directory.Create();
        }
    }
}
