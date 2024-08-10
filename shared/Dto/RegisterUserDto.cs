using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shared.Dto
{
    public class RegisterUserDto
    {

        public string userName { get; set; }
        /// <summary>
        /// 用户昵称
        /// </summary>
        public string nickName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string password { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string phoneNumber { get; set; }
        /// <summary>
        /// 1启用 0禁用
        /// </summary>
        public int disabled { get; set; }

        public List<long> roleIds { set; get; }

    }
}
