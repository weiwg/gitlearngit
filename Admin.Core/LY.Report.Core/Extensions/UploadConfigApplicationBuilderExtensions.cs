using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using LY.Report.Core.Common.Configs;
using Microsoft.Extensions.FileProviders;

namespace LY.Report.Core.Extensions
{
    /// <summary>
    /// 上传配置
    /// </summary>
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

        public static IApplicationBuilder UseUploadConfig(this IApplicationBuilder app)
        {
            var uploadConfig = app.ApplicationServices.GetRequiredService<IOptions<UploadConfig>>();
            UseFileUploadConfig(app, uploadConfig.Value.Avatar);
            UseFileUploadConfig(app, uploadConfig.Value.Document);
            UseFileUploadConfig(app, uploadConfig.Value.Certificate);

            return app;
        }
    }

}
