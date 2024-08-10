using api.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace api.Bll.impl
{
    [Service(ServiceLifetime.Singleton)]
    public class SysMenuBll : ISysMenuBll
    {
    }
}
