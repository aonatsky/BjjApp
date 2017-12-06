using System;
using TRNMNT.Core.Enum;

namespace TRNMNT.Core.Model
{
    public class FileProcessResult
    {
        public FileProcessResult()
        {

        }
        public FileProcessResult(FileProcessResultEnum code, string message = "")
        {
            Code = code;
            Message = message;
            if (string.IsNullOrEmpty(message))
            {
                switch (code)
                {
                    case FileProcessResultEnum.FileIsInvalid:
                        message = "File is not valid";
                        break;
                    case FileProcessResultEnum.FileIsNull:
                        message = "File is null";
                        break;
                    case FileProcessResultEnum.Error:
                        message = "An error occured during processing";
                        break;
                    case FileProcessResultEnum.FileIsEmpty:
                        message = "File is empty";
                        break;
                }
            }

        }
        public FileProcessResultEnum Code { get; set; }
        public string Message { get; set; }
    }

}