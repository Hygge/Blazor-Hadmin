using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shared.Dto
{
    public class RoleDto
    {
        public string roleName { get; set; }
        /// <summary>
        /// 角色权限字符
        /// </summary>
        public string roleKey { get; set; }

        /// <summary>
        /// 1启用 0禁用
        /// </summary>
        public int disabled { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string remark { set; get; }

    }
}
