using System;
using Yitter.IdGenerator;

namespace api.Domain
{
    public class SysRole
    {

        /// <summary>
        /// id
        /// </summary>
        public long id { get; set; } = YitIdHelper.NextId();
        /// <summary>
        /// 角色名称
        /// </summary>
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
        /// 创建人
        /// </summary>
        public string createdBy { set; get; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime createdTime { set; get; }

        /// <summary>
        /// 备注
        /// </summary>
        public string remark { set; get; }

    }
}
