using api.Domain;
using shared.Dto;
using shared.Utils;

namespace api.Bll
{
    public interface ISysRoleBll
    {

        public void Save(RoleDto dto, string createdBy);

        public Pager<SysRole> Page(string? roleName, int pageNum, int pageSize);


    }
}
