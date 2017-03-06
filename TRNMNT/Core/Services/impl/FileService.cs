using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace TRNMNT.Core.Services
{
    public class FileService
    {
        private IHostingEnvironment env;
        public FileService(IHostingEnvironment env)
        {
            this.env = env;
        }

        public bool SaveFile(string fileType, Stream stream)
        {
            return true;
        }

        public string GetWebRootPath()
        {
            return this.env.WebRootPath;
        }



    }
}

