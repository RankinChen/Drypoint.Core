using Drypoint.Unity.OptionsConfigModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Drypoint.Core.Extensions
{
    public static class UploadConfigApplicationBuilderExtensions
    {
        private static void UseFileUploadConfig(IApplicationBuilder app, FileUploadConfig config)
        {
            if (!Directory.Exists(config.UploadPath))
            {
                Directory.CreateDirectory(config.UploadPath);
            }

            app.UseStaticFiles(new StaticFileOptions()
            {
                RequestPath = config.RequestPath,
                FileProvider = new PhysicalFileProvider(config.UploadPath)
            });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseUploadConfig(this IApplicationBuilder app)
        {
            var uploadConfig = app.ApplicationServices.GetRequiredService<IOptions<UploadConfig>>();
            UseFileUploadConfig(app, uploadConfig.Value.Avatar);
            UseFileUploadConfig(app, uploadConfig.Value.Document);

            return app;
        }
    }
}
