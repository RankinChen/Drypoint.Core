using Drypoint.Unity.Enums;
using Drypoint.Unity.Extensions;
using Drypoint.Unity.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Drypoint.Unity.SharedModels
{
    /// <summary>
    /// 文件信息
    /// </summary>
    public class FileInfoModel
    {
        public FileInfoModel()
        {
        }

        /// <summary>
        /// 初始化文件信息
        /// </summary>
        /// <param name="fileName">文件名称</param>
        /// <param name="size">大小</param>
        public FileInfoModel(string fileName, long size = 0L)
        {
            FileName = fileName;
            Size = new FileSize(size);
            Extension = System.IO.Path.GetExtension(FileName)?.TrimStart('.');
        }

        /// <summary>
        /// 上传路径
        /// </summary>
        public string UploadPath { get; set; }

        /// <summary>
        /// 请求路径
        /// </summary>
        public string RequestPath { get; set; }

        /// <summary>
        /// 相对路径
        /// </summary>
        public string RelativePath { get; set; }

        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 保存名
        /// </summary>
        public string SaveName { get; set; }

        /// <summary>
        /// 文件大小
        /// </summary>
        public FileSize Size { get; set; }

        /// <summary>
        /// 扩展名
        /// </summary>
        public string Extension { get; set; }

        /// <summary>
        /// 文件目录
        /// </summary>
        public string FileDirectory => System.IO.Path.Combine(UploadPath, RelativePath).ToPath();

        /// <summary>
        /// 文件请求路径
        /// </summary>
        public string FileRequestPath => System.IO.Path.Combine(RequestPath, RelativePath, SaveName).ToPath();

        /// <summary>
        /// 文件相对路径
        /// </summary>
        public string FileRelativePath => System.IO.Path.Combine(RelativePath, SaveName).ToPath();

        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath => System.IO.Path.Combine(UploadPath, RelativePath, SaveName).ToPath();
    }


    /// <summary>
    /// 文件大小
    /// </summary>
    public struct FileSize
    {
        /// <summary>
        /// 初始化文件大小
        /// </summary>
        /// <param name="size">文件大小</param>
        /// <param name="unit">文件大小单位</param>
        public FileSize(long size, FileSizeUnit unit = FileSizeUnit.Byte)
        {
            switch (unit)
            {
                case FileSizeUnit.KB:
                    _size = size * 1024; break;
                case FileSizeUnit.MB:
                    _size = size * 1024 * 1024; break;
                case FileSizeUnit.GB:
                    _size = size * 1024 * 1024 * 1024; break;
                default:
                    _size = size; break;
            }
        }

        /// <summary>
        /// 文件字节长度
        /// </summary>
        public long _size { get; }

        /// <summary>
        /// 获取文件大小，单位：字节
        /// </summary>
        public long GetSize()
        {
            return _size;
        }

        /// <summary>
        /// 获取文件大小，单位：K
        /// </summary>
        public double GetSizeByK()
        {
            return (_size / 1024.0).ToDouble(2);
        }

        /// <summary>
        /// 获取文件大小，单位：M
        /// </summary>
        public double GetSizeByM()
        {
            return (_size / 1024.0 / 1024.0).ToDouble(2);
        }

        /// <summary>
        /// 获取文件大小，单位：G
        /// </summary>
        public double GetSizeByG()
        {
            return (_size / 1024.0 / 1024.0 / 1024.0).ToDouble(2);
        }

        /// <summary>
        /// 输出描述
        /// </summary>
        public override string ToString()
        {
            if (_size >= 1024 * 1024 * 1024)
                return $"{GetSizeByG()} {FileSizeUnit.GB.ToDescription()}";
            if (_size >= 1024 * 1024)
                return $"{GetSizeByM()} {FileSizeUnit.MB.ToDescription()}";
            if (_size >= 1024)
                return $"{GetSizeByK()} {FileSizeUnit.KB.ToDescription()}";
            return $"{_size} {FileSizeUnit.Byte.ToDescription()}";
        }
    }
}
