using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using TRNMNT.Core.Enum;
using Microsoft.AspNetCore.Http;

namespace TRNMNT.Core.Services
{
    public class FileService
    {
        private const string FIGHTERLIST_FOLDER = "\\FighterList";
        private const string FIGHTERLIST_FILE = "\\List";
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

        public string GetFighterListPath()
        {
            return Path.Combine(GetWebRootPath(), FIGHTERLIST_FOLDER, $"{FIGHTERLIST_FILE}_{DateTime.UtcNow.ToString("yyyy.mm.dd")}");
        }

        public FileProcessResultEnum ValidateFile(IFormFile file, FileTypeEnum fileType)
        {
            if (file == null)
            {
                return FileProcessResultEnum.FileIsNull;
            }
            if (file.Length == 0)
            {
                return FileProcessResultEnum.FileIsEmpty;
            }

            switch (fileType)
            {
                case FileTypeEnum.FighterList:
                    {
                        using (var fileStream = new FileStream(GetFighterListPath(), FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }
                        return FileProcessResultEnum.Success;
                    };
                default:
                    {
                        return FileProcessResultEnum.FileIsInvalid;
                    }
            }
        }

        public Stream GetStream(IFormFile file){
            using (Stream stream = file.OpenReadStream())
                {
                   return stream;
                }
        }
    }
}

