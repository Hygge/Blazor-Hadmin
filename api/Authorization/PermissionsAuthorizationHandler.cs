using api.Attributes;
using api.Config.Db;
using api.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace api.Authorization
{
    public class PermissionsAuthorizationHandler : AuthorizationHandler<PermissionsAttribute>
    {
        private readonly ILogger<PermissionsAuthorizationHandler> _logger;
        private readonly IMongoDatabase mongoDatabase;
        public PermissionsAuthorizationHandler(ILogger<PermissionsAuthorizationHandler> logger, IOptions<MongodbSettings> options1)
        {
            _logger = logger;
            var mongoClient = new MongoClient(options1.Value.ConnectionString);

            mongoDatabase = mongoClient.GetDatabase(options1.Value.DatabaseName);
        }

        /// <summary>
        /// 自定义授权校验方法
        /// </summary>
        /// <param name="context"></param>
        /// <param name="requirement">权限字符串</param>
        /// <returns></returns>
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            PermissionsAttribute requirement)
        {
            // 认证未通过就不用往下走了
            if (!context.User.Identity.IsAuthenticated)
            {
                return Task.CompletedTask;
            }

            _logger.LogWarning("权限字符校验-- {Key}", requirement.Key);

            if (string.IsNullOrEmpty(requirement.Key))
            {
                //放行
                context.Succeed(requirement);
            }
           

            // todo 获取当前用户的角色，去数据库查询角色对应的权限表中是否有当前接口授权注解的权限字符串，没有不让通过有就通过
            var userId = int.Parse(context.User?.Claims?.FirstOrDefault(c => c.Type == "Id").Value); // 这里结合token给的
            var userName = context.User.Claims.FirstOrDefault(c => c.Type == "userName").Value;

            var users = mongoDatabase.GetCollection<SysUser>(typeof(SysUser).Name).AsQueryable();


            if (userName.Equals("admin"))
            {
                //放行
                context.Succeed(requirement);
            }
            try
            {
                var userAndRole = mongoDatabase.GetCollection<SysUserAndRole>(typeof(SysUserAndRole).Name).AsQueryable();
                var roleAndMenu = mongoDatabase.GetCollection<SysRoleAndMenu>(typeof(SysRoleAndMenu).Name).AsQueryable();
                var menus = mongoDatabase.GetCollection<SysMenu>(typeof(SysMenu).Name).AsQueryable();
                var query = from user in users
                            join ur in userAndRole on user.id equals ur.userId
                            join mr in roleAndMenu on ur.roleId equals mr.roleId
                            join menu in menus on mr.menuId equals menu.id
                            where user.id.Equals(userId)
                            select menu.psm;

                List<string> perms = query.ToList();
                if (perms.Count() == 0)
                {
                    return Task.CompletedTask;
                }
                if (perms.Any(p => p.Equals(requirement.Key)))
                {
                    //放行
                    context.Succeed(requirement);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"权限校验失败请检查数据库：{ex.Message}");
            }
            finally
            {

            }          

            return Task.CompletedTask;
        }
    }
}
