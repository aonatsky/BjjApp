using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace TRNMNT.Core.Services
{
    public class FileProcessingService
    {
        private IHostingEnvironment env;
        public FileProcessingService(IHostingEnvironment env)
        {
            this.env = env;
        }

        public bool SaveFile(string fileType, Stream stream)
        {
            string name = $"{fileType.ToString()}_{DateTime.UtcNow.ToString("yyyy.mm.dd")}";
            return true;
        }

private string GetWebRootPath()
{
    return this.env.WebRootPath;
}
    }
}

