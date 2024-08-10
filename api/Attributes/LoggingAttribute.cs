using System;
using Microsoft.AspNetCore.Mvc.Filters;

namespace api.Attributes
{
    public class LoggingAttribute : ActionFilterAttribute
    {
        private readonly string methodDesc;
        private long logId;
        public LoggingAttribute(string methodDesc) => this.methodDesc = methodDesc;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // 在执行操作方法之前执行的逻辑
            Console.WriteLine("Before executing action " + methodDesc);
            logId = 210L;
            base.OnActionExecuting(context);
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            // 在执行操作方法之后执行的逻辑
            Console.WriteLine("After executing action " + methodDesc);
            base.OnActionExecuted(context);

        }

    }

}
