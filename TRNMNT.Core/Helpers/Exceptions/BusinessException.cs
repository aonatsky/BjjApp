using System;
namespace TRNMNT.Core.Helpers.Exceptions
{
    public class BusinessException : System.Exception
    {
        public BusinessException(string message) : base(message)
        {

        }
    }
}