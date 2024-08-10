namespace api.Attributes

{
    /// <summary>
    /// 属性形式定义生命周期
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ServiceAttribute : Attribute
    {
        public ServiceLifetime LifeTime { get; set; }
        public ServiceAttribute(ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        {
            LifeTime = serviceLifetime;
        }
    }
}
