using Drypoint.Unity.BaseServices;
using Drypoint.Unity.Extensions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Drypoint.Core.Extensions.BaseServices
{
    /// <summary>
    /// IPHelper实现类
    /// </summary>
    public class IPHelper : IIPPHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IPHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetMACIP()
        {
            //本地计算机网络连接信息
            //IPGlobalProperties computerProperties = IPGlobalProperties.GetIPGlobalProperties();
            //获取本机电脑名
            //var HostName = computerProperties.HostName;
            //获取域名
            //var DomainName = computerProperties.DomainName;

            //获取本机所有网络连接
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();

            if (nics == null || nics.Length < 1)
            {
                return "";
            }
            var MACIp = "";
            foreach (NetworkInterface adapter in nics)
            {
                var adapterName = adapter.Name;

                var adapterDescription = adapter.Description;
                var NetworkInterfaceType = adapter.NetworkInterfaceType;
                if (adapterName.Contains("本地连接") || adapterName == "WLAN")
                {
                    PhysicalAddress address = adapter.GetPhysicalAddress();
                    byte[] bytes = address.GetAddressBytes();

                    for (int i = 0; i < bytes.Length; i++)
                    {
                        MACIp += bytes[i].ToString("X2");

                        if (i != bytes.Length - 1)
                        {
                            MACIp += "-";
                        }
                    }
                }
            }
            return MACIp;
        }

        public string GetRequestIP()
        {
            var request = _httpContextAccessor.HttpContext.Request;
            if (request == null)
            {
                return "";
            }

            string ip = request.Headers["X-Real-IP"].FirstOrDefault();
            if (ip.IsNull())
            {
                ip = request.Headers["X-Forwarded-For"].FirstOrDefault();
            }
            if (ip.IsNull())
            {
                ip = request.HttpContext?.Connection?.RemoteIpAddress?.ToString();
            }
            if (ip.IsNull() || !IsIP(ip.Split(":")[0]))
            {
                ip = "127.0.0.1";
            }

            return ip;
        }

        public bool IsIP(string ip)
        {
            return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }
    }
}
