using System;
using System.IO;

namespace TRNMNT.Core.Service
{
    public static class FilePathService
    {
        private const string FIGHTERLIST_FOLDER_NAME = "FighterList";
        public const string FIGHTERLIST_FILE_NAME = "List";

        private const string BRACKETS_FILE_NAME_MASK = "brackets-";
        private const string BRACKETS_FILE_FOLDER_NAME = "FileSamples";



        public static string GetFighterListUploadFolder(string rootpath)
        {
            return Path.Combine(rootpath, FIGHTERLIST_FILE_NAME);
        }

        public static string GetFighterListFilePath(string rootpath)
        {
            return Path.Combine(GetFighterListUploadFolder(rootpath), $"{FIGHTERLIST_FILE_NAME}_{DateTime.UtcNow.ToString("yyyy.mm.dd")}.xlsx");
        }

        public static string GetBracketsFilePath(string rootpath, int size)
        {
            return Path.Combine(rootpath, BRACKETS_FILE_FOLDER_NAME, $"{BRACKETS_FILE_NAME_MASK}{size.ToString()}.xlsx");
        }



    }
}
