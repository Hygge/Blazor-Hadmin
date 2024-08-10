using api.Attributes;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Authorization;
using MongoDB.Bson.IO;
using System.Text.Json;
using shared.Utils;

namespace api.Authorization
{
    public class CustomizeAuthorizationMiddlewareResultHandler : IAuthorizationMiddlewareResultHandler
    {
        private readonly AuthorizationMiddlewareResultHandler defaultHandler = new();

        public async Task HandleAsync(
            RequestDelegate next,
            HttpContext context,
            AuthorizationPolicy policy,
            PolicyAuthorizationResult authorizeResult)
        {

            if (authorizeResult.Forbidden && authorizeResult.AuthorizationFailure!.FailedRequirements
                    .OfType<PermissionsAttribute>().Any())
            {
          
                var payload = JsonSerializer.Serialize(new { Code = HttpCode.PERMISSIONS_CODE, Message = "很抱歉，您暂无权限访问该接口！" });
                //自定义返回的数据类型
                context.Response.ContentType = "application/json";
                //自定义返回状态码，默认为401 我这里改成 200
                context.Response.StatusCode = StatusCodes.Status200OK;
                //输出Json数据结果
                context.Response.WriteAsync(payload);
                return;
            }

            // Fall back to the default implementation.
            await defaultHandler.HandleAsync(next, context, policy, authorizeResult);
        }
    }

}
