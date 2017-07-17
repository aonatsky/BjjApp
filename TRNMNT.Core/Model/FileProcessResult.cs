using System;
using TRNMNT.Web.Core.Enum;

namespace TRNMNT.Web.Core.Model
{
    public class FileProcessResult
    {
        public FileProcessResult()
        {

        }
        public FileProcessResult(FileProcessResultEnum code, string message = "")
        {
            this.Code = code;
            this.Message = message;
            if (String.IsNullOrEmpty(message))
            {
                if (code == FileProcessResultEnum.FileIsInvalid)
                {
                    message = "File is not valid";
                }
                if (code == FileProcessResultEnum.FileIsNull)
                {
                    message = "File is null";
                }
                if (code == FileProcessResultEnum.Error)
                {
                    message = "An error occured during processing";
                }
                if (code == FileProcessResultEnum.FileIsEmpty)
                {
                    message = "File is empty";
                }

            }

        }
        public FileProcessResultEnum Code { get; set; }
        public string Message { get; set; }
    }

}