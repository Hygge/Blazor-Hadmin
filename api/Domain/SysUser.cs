using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Yitter.IdGenerator;

namespace api.Domain
{
    public class SysUser
    {
        /// <summary>
        /// id
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.Int64)]
        public long id { get; set; } = YitIdHelper.NextId();
        /// <summary>
        /// 用户名称
        /// </summary>
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
        /// <summary>
        /// 创建人
        /// </summary>
         public string createdBy { set; get; }
        /// <summary>
        /// 创建时间
        /// </summary>
         public DateTime createdTime { set; get; }



    }
}
