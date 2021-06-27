using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Drypoint.Core.Extensions
{
    public static class AutoMapperExtensions
    {
        public static void AddCustomAutoMapper(this IServiceCollection services)
        {
            var profileTypes =
                from assembly in AppDomain.CurrentDomain.GetAssemblies()
                from type in assembly.GetTypes()
                where type.IsSubclassOf(typeof(Profile)) && !type.IsGenericType && !type.IsAbstract
                select type;

            var profiles = profileTypes.Select(x =>
            {
                try
                {
                    return (Profile)Activator.CreateInstance(x);
                }
                catch (MissingMethodException ex)
                {
                    return null;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }).Where(x => x != null);

            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfiles(profiles);
            });
        }
    }
}
