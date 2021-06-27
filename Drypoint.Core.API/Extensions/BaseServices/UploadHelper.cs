using Drypoint.Unity.BaseServices;
using Drypoint.Unity.Extensions;
using Drypoint.Unity.Helpers;
using Drypoint.Unity.OptionsConfigModels;
using Drypoint.Unity.SharedModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Yitter.IdGenerator;

namespace Drypoint.Core.Extensions.BaseServices
{
    /// <summary>
    /// 上传文件服务
    /// </summary>
    public class UploadHelper : IUploadHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly FileUploadConfig fileUploadConfig;

        public UploadHelper(IHttpContextAccessor httpContextAccessor, IOptions<FileUploadConfig> fileUploadConfig)
        {
            _httpContextAccessor = httpContextAccessor;
            this.fileUploadConfig = fileUploadConfig.Value;
        }

        /// <summary>
        /// 保存到指定位置
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task SaveAsync(string filePath, CancellationToken cancellationToken = default)
        {
            var files = _httpContextAccessor.HttpContext.Request?.Form?.Files;
            if (files != null && files.Any())
            {
                using var stream = new FileStream(filePath, FileMode.Create);
                await files[0].CopyToAsync(stream, cancellationToken);
            }
        }

        /// <summary>
        /// 根据配置文件保存位置
        /// </summary>
        /// <param name="args"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<MessageModel<FileInfoModel>> UploadAsync(object args, CancellationToken cancellationToken = default)
        {
            var files = _httpContextAccessor.HttpContext.Request?.Form?.Files;

            var res = new MessageModel<FileInfoModel>();

            if (files != null && files.Any())
            {
                return res.BuildFailMessage("请上传文件！");
            }

            //格式限制
            if (!fileUploadConfig.ContentType.Contains(files[0].ContentType))
            {
                return res.BuildFailMessage("文件格式错误");
            }

            //大小限制
            if (files[0].Length > fileUploadConfig.MaxSize)
            {
                return res.BuildFailMessage("文件过大");
            }

            var fileInfo = new FileInfoModel(files[0].FileName, files[0].Length)
            {
                UploadPath = fileUploadConfig.UploadPath,
                RequestPath = fileUploadConfig.RequestPath
            };

            var dateTimeFormat = fileUploadConfig.DateTimeFormat.NotNull() ? DateTime.Now.ToString(fileUploadConfig.DateTimeFormat) : "";
            //var format = config.Format.NotNull() ? StringHelper.Format(config.Format, args) : "";
            fileInfo.RelativePath = Path.Combine(dateTimeFormat, fileUploadConfig.Format).ToPath();

            if (!Directory.Exists(fileInfo.FileDirectory))
            {
                Directory.CreateDirectory(fileInfo.FileDirectory);
            }

            fileInfo.SaveName = $"{YitIdHelper.NextId()}.{fileInfo.Extension}";


            using var stream = new FileStream(path: fileInfo.FilePath, mode: FileMode.Create);
            await files[0].CopyToAsync(stream, cancellationToken);
            return res.BuildSuccessMessage();
        }
    }
}
