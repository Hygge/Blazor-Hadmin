using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shared.Dto
{
    public class MenuDto
    {
        public long id { get; set; }
        /// <summary>
        /// 父级菜单id
        /// </summary>
        public long parentId { get; set; } = 0;
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 1目录 2菜单 3按钮
        /// </summary>
        public int type { get; set; }
        /// <summary>
        /// 图标字符串
        /// </summary>
        public string icon { get; set; }
        /// <summary>
        /// 路由地址
        /// </summary>
        public string route { get; set; }
        /// <summary>
        /// 权限字符串
        /// </summary>
        public string psm { get; set; }
        /// <summary>
        /// 1启用 0禁用
        /// </summary>
        public int disabled { get; set; }

        public List<MenuDto> children { set; get; } = new List<MenuDto>();
    }
}
