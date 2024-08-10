using api.Bll;
using api.Exceptions;
using api.Result;
using api.Util;
using Microsoft.AspNetCore.Mvc;
using shared.Dto;

namespace api.Controller
{
    [ApiController]
    [Route("[controller]")]
    public class RoleController : ControllerBase
    {

        private readonly ILogger<RoleController> _logger;
        private readonly HttpContext httpContext;
        private readonly ISysRoleBll roleBll;
        public RoleController(ILogger<RoleController> logge, IServiceProvider serviceProvider, IHttpContextAccessor httpContextAccessor)
        {
            this._logger = logge;
            this.roleBll = serviceProvider.GetRequiredService<ISysRoleBll>();
            httpContext = httpContextAccessor.HttpContext;
        }

        [HttpPost]
        public ApiResult AddRole([FromBody] RoleDto roleDto)
        {
            if (string.IsNullOrEmpty(roleDto.roleName)){
                throw new BusinessException("角色名不能为空");
            }
            //HttpContextUtil.getUserName(HttpContext)
            roleBll.Save(roleDto, "script");
            

            return ApiResult.succeed(roleDto);
        }

        [HttpGet]
        public ApiResult List(string? roleName, int pageNum = 1, int pageSize = 5)
        {
            
            return ApiResult.succeed(roleBll.Page(roleName, pageNum, pageSize));
        }



    }
}
