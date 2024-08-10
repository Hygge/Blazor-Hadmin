using api.Attributes;
using System.Reflection;

namespace api.Extension
{
    public static class ServiceCollectionExptension
    {

        public static void AddBusiness(this IServiceCollection services)
        {
            List<Type> types = AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(t => t.IsClass && !t.IsAbstract && t.GetCustomAttributes(typeof(ServiceAttribute), false).Length > 0)
                .ToList();

            types.ForEach(impl =>
            {
                Type[] interfaces = impl.GetInterfaces();

                var lifeTime = impl.GetCustomAttribute<ServiceAttribute>().LifeTime;

                interfaces.ToList().ForEach(i =>
                {
                    switch (lifeTime)
                    {
                        case ServiceLifetime.Singleton:
                            services.AddSingleton(i, impl);
                            break;
                        case ServiceLifetime.Scoped:
                            services.AddScoped(i, impl);
                            break;
                        case ServiceLifetime.Transient:
                            services.AddTransient(i, impl);
                            break;
                    }
                });
            });
        }

    }
}
