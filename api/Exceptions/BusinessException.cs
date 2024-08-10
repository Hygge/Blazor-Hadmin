using System;
using shared.Utils;

namespace api.Exceptions
{
    public class BusinessException : ApplicationException
    {
        public int code { set; get; }
        public string message { set; get; }

        public BusinessException(int code, string message)
        {
            this.code = code;
            this.message = message;
        }

        public BusinessException(string message)
        {
            this.code = HttpCode.FAILED_CODE;
            this.message = message;
        }

    }

}
