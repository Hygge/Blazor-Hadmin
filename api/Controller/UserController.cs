using api.Bll;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using shared.Dto;

namespace api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ISysUserBll _userBll;
        public UserController(ISysUserBll sysUserBll)
        {
            _userBll = sysUserBll;
        }

        [HttpPost]
        public string Add([FromBody] RegisterUserDto userDto)
        {
            _userBll.saveUser(userDto, "script");
            return "ok";
        } 


    }
}
