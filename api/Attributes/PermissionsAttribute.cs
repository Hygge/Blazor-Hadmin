using Microsoft.AspNetCore.Authorization;

namespace api.Attributes
{
    /// <summary>
    /// 权限特性
    /// </summary>
    public class PermissionsAttribute : AuthorizeAttribute, IAuthorizationRequirement, IAuthorizationRequirementData
    {
        public PermissionsAttribute(string key) => Key = key;
        public string Key { get; }

        public IEnumerable<IAuthorizationRequirement> GetRequirements()
        {
            yield return this;
        }

    }

}
