using api.Attributes;
using api.Config.Db;
using api.Domain;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using shared.Dto;
using shared.Utils;

namespace api.Bll.impl
{
    [Service(ServiceLifetime.Singleton)]
    public class SysUserBll : ISysUserBll
    {

        private readonly IMongoCollection<SysUser> _sysUserc;
        private readonly IMongoDatabase mongoDatabase;
        private readonly ILogger<SysUserBll> logger;
        public SysUserBll(IOptions<MongodbSettings> options1, ILogger<SysUserBll> logger1)
        {
            var mongoClient = new MongoClient(options1.Value.ConnectionString);

            mongoDatabase = mongoClient.GetDatabase(options1.Value.DatabaseName);
            _sysUserc = mongoDatabase.GetCollection<SysUser>(typeof(SysUser).Name);

            logger = logger1;
        }

        public void saveUser(RegisterUserDto userDto, string createBy)
        {
            SysUser u = _sysUserc.AsQueryable().Where(item => userDto.userName.Equals(item.userName)).SingleOrDefault();
            if(u != null)
            {
                throw new Exception("用户名已存在");
            }
            SysUser sysUser = new SysUser();
            sysUser.userName = userDto.userName;
            sysUser.nickName = userDto.nickName;
            sysUser.password = EncryptUtil.SHA256Encrypt(userDto.password);
            sysUser.phoneNumber = userDto.phoneNumber;
            sysUser.createdBy = createBy;
            sysUser.createdTime = DateTime.Now;

            _sysUserc.InsertOne(sysUser);



        }
    }
}
