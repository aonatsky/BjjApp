using TRNMNT.Core.Enum;

namespace TRNMNT.Core.Model
{
    public class FileProcessResult
    {
    public FileProcessResult()
    {

    }
    public FileProcessResult(FileProcessResultEnum result, string message = "")
    {
        this.Result = result;
        this.Message = message;
    }
        public  FileProcessResultEnum Result {get;set;}
        public string Message {get;set;}
    }
   
}