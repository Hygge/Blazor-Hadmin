using shared.Dto;

namespace api.Bll
{
    public interface ISysUserBll
    {

        public void saveUser(RegisterUserDto userDto, string createBy);

    }
}
