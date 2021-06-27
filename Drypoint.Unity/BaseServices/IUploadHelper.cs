using Drypoint.Unity.SharedModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Drypoint.Unity.BaseServices
{
    /// <summary>
    /// 保存当前文件流
    /// </summary>
    public interface IUploadHelper
    {
        /// <summary>
        /// 上传当前请求的文件
        /// </summary>
        /// <param name="args"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<MessageModel<FileInfoModel>> UploadAsync(object args, CancellationToken cancellationToken = default);

        /// <summary>
        /// 保存文件到指定位置
        /// </summary>
        /// <param name="file"></param>
        /// <param name="filePath"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task SaveAsync(string filePath, CancellationToken cancellationToken = default);
    }
}
