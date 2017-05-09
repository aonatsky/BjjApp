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
        this.Code = code;
        this.Message = message;
    }
        public  FileProcessResultEnum Code {get;set;}
        public string Message {get;set;}
    }
   
}