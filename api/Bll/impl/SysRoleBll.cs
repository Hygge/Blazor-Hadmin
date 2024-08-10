using System;
using System.Collections.Generic;
using Amazon.Runtime.Internal;
using api.Attributes;
using api.Config.Db;
using api.Domain;
using api.Exceptions;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using shared.Dto;
using shared.Utils;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace api.Bll.impl
{
    [Service(ServiceLifetime.Singleton)]
    public class SysRoleBll : ISysRoleBll
    {

        private readonly IMongoCollection<SysRole> sysRole;
        private readonly IMongoDatabase mongoDatabase;
        private readonly ILogger<SysUserBll> logger;
        public SysRoleBll(IOptions<MongodbSettings> options1, ILogger<SysUserBll> logger1)
        {
            var mongoClient = new MongoClient(options1.Value.ConnectionString);

            mongoDatabase = mongoClient.GetDatabase(options1.Value.DatabaseName);
            sysRole = mongoDatabase.GetCollection<SysRole>(typeof(SysRole).Name);

            logger = logger1;
        }

        public Pager<SysRole> Page(string? roleName, int pageNum, int pageSize)
        {
            Pager<SysRole> page = new Pager<SysRole>(pageNum, pageSize);
            List<SysRole> roleList;
            if(!string.IsNullOrEmpty(roleName))
            {
                var query = sysRole.AsQueryable().Where(item => item.roleName.Contains(roleName))
                     .Skip(page.getSkip()).Take(pageSize);
                page.rows = query.ToList();
                page.total = query.Count();
            }
            else
            {
               /* var query = sysRole.Find(_ => true).Skip(page.getSkip()).Limit(pageSize);
                page.rows = query.ToList();
                page.total = sysRole.Find(_ => true).Count();*/
               var query = sysRole.AsQueryable()
                     .Skip(page.getSkip()).Take(pageSize);
                page.rows = query.ToList();
                page.total = query.Count();
            }

            return page;
        }

        public void Save(RoleDto dto, string createdBy)
        {
           SysRole r = sysRole.AsQueryable().Where(x => x.roleKey.Equals(dto.roleKey) || x.roleName.Equals(dto.roleName)).SingleOrDefault();
            if (r != null)
            {
                throw new BusinessException("角色名称或角色字符已存在");
            }

            SysRole role = new();
            role.roleKey = dto.roleKey;
            role.roleName = dto.roleName;
            role.remark = dto.remark;
            role.disabled = dto.disabled;
            role.createdBy = createdBy;
            role.createdTime = DateTime.Now;

            sysRole.InsertOne(role);

        }





    }
}
