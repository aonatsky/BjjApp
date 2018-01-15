using System;
using System.Collections.Generic;
using TRNMNT.Core.Enum;

namespace TRNMNT.Core.Model
{
    public class FileProcessResult
    {
        public FileProcessResult()
        {

        }
        public FileProcessResult(FileProcessResultEnum code, List<string> messages = null)
        {
            Code = code;
            if (messages == null)
            {
                var message = string.Empty;
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
                Messages = new List<string> {message};
            }
            else
            {
                Messages = messages;
            }
        }

        public FileProcessResult(FileProcessResultEnum code, string message) : this(code, new List<string> {message})
        {
        }

        public FileProcessResultEnum Code { get; set; }
        public List<string> Messages { get; set; }
    }

}