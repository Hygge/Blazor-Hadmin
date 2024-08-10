using shared.Utils;

namespace api.Result
{
    public class ApiResult
    {

        public int code { get; set; } = HttpCode.SUCCESS_CODE;
        public string message { get; set; } = "ok！";
        public object? data { get; set; }
        public string timestamp { get; set; } = DateUtil.getCurrentDateTimeFormat();


        public ApiResult() { }
        public ApiResult(int code, string message, object? data)
        {
            this.code = code;
            this.message = message;
            this.data = data;
        }
        public ApiResult(int code, string message)
        {
            this.code = code;
            this.message = message;
        }
        public ApiResult(int code, object? data)
        {
            this.code = code;
            this.data = data;
        }

        public static ApiResult succeed(object? data = null)
        {
            return new ApiResult(HttpCode.SUCCESS_CODE, data);
        }
        public static ApiResult failed(int code, string message)
        {
            return new ApiResult(code, message);
        }


    }

}
