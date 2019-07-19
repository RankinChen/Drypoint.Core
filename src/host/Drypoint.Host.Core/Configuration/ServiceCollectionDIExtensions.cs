using Drypoint.EntityFrameworkCore.EntityFrameworkCore;
using Drypoint.EntityFrameworkCore.Repositories;
using Drypoint.Unity.Dependency;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Drypoint.Host.Core.Configuration
{
    /// <summary>
    /// ServiceCollection扩展类
    /// 注入实现ISingletonDependency和ITransientDependency接口的实现
    /// </summary>
    public static class ServiceCollectionDIExtensions
    {
        public static void AddServiceRegister(this IServiceCollection services)
        {
            try
            {
                services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
                services.TryAddTransient(typeof(IDesignTimeDbContextFactory<DrypointDbContext>), typeof(DrypointDbContextFactory)); 
                services.TryAddTransient(typeof(IRepository<,>), typeof(DrypointBaseRepository<,>));
                AddCommonService(services);


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void AddCommonService(IServiceCollection  services)
        {
            AddService(services, "Drypoint.Host.Core");
            AddService(services, "Drypoint.Application");
            AddService(services, "Drypoint.Application.Custom");
            AddService(services, "Drypoint.EntityFrameworkCore");
        }

        private static void AddService(IServiceCollection services,string assemblyName)
        {
            Assembly assembly = null;

            try
            {
                //不注入未引用的类
                assembly = Assembly.Load(assemblyName);
            }
            catch (Exception ex)
            {
                return ;
            }

            List<Type> ts = assembly.GetTypes().ToList();

            foreach (var item in ts.Where(d => d.IsClass))
            {
                if (item.Namespace == null || item.IsGenericType)
                {
                    continue;
                }
                if (item.IsClass && item.Namespace.StartsWith(assemblyName))
                {

                    List<Type> ltInterface = item.GetInterfaces().Where(d => !d.IsGenericType).ToList();
                    if (ltInterface == null || ltInterface.Count < 1)
                    {
                        continue;
                    }
                    if (ltInterface.FirstOrDefault(d => d == typeof(ISingletonDependency)) != null)
                    {
                        Type itface = ltInterface.FirstOrDefault(d => d.GetType() != typeof(ISingletonDependency));
                        services.TryAddSingleton(itface, item);
                    }
                    else
                    {
                        services.AddTransient(ltInterface.FirstOrDefault(), item);
                    }
                }

            }
        }
    }
}
