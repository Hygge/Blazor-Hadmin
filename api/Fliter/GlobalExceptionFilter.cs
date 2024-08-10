using System.Threading.Tasks;
using api.Exceptions;
using api.Result;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace api.Fliter
{


    public class GlobalExceptionFilter : IAsyncExceptionFilter
    {
        private readonly ILogger<GlobalExceptionFilter> _logger;

        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
        {
            _logger = logger;
        }



        public Task OnExceptionAsync(ExceptionContext context)
        {

            switch (context.Exception)
            {
                case BusinessException:
                    var h = context.Exception as BusinessException;
                    context.Result = new ObjectResult(ApiResult.failed(h.code, h.message));
                    _logger.LogError(h.message);
                    break;
                default:
                    context.Result = new ObjectResult(ApiResult.failed(500, context.Exception.Message));
                    _logger.LogError(context.Exception.Message);
                    break;
            }

            context.ExceptionHandled = true;
            return Task.CompletedTask;
        }

    }

}
