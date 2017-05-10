using System;
using System.IO;

namespace TRNMNT.Core.Service
{
    public static class FilePathService
    {
        private const string FIGHTERLIST_FOLDER_NAME = "FighterList";
        public const string FIGHTERLIST_FILE_NAME = "List";
        private const string DATE_FORMAT = "dd-mm-yy";

        public static string GetFighterListUploadFolder(string rootpath)
        {
            return Path.Combine(rootpath, FIGHTERLIST_FILE_NAME);
        }

        public static string GetFighterListFilePath(string rootpath)
        {
            return Path.Combine(GetFighterListUploadFolder(rootpath), $"{FIGHTERLIST_FILE_NAME}_{DateTime.UtcNow.ToString("yyyy.mm.dd")}.xlsx");
        }
    }
}
