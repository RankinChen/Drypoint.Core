using System;
using System.Collections.Generic;
using System.Text;

namespace Drypoint.Unity.OptionsConfigModels
{
    /// <summary>
    /// 认证授权配置，从appsettings.json读取
    /// </summary>

    public class AuthManagement
    {
        public IdentityServer IdentityServer { get; set; }

        public JwtBearer JwtBearer { get; set; }

        public AuthManagement()
        {
            IdentityServer = new IdentityServer();
            JwtBearer = new JwtBearer();
        }
    }

    public class IdentityServer
    {
        public bool IsEnabled { get; set; }
        public string Authority { get; set; }
        public string ApiName { get; set; }
        public string ApiSecret { get; set; }

    }

    public class JwtBearer
    {
        public bool IsEnabled { get; set; }
        public string SecurityKey { get; set; }

        public string Issuer { get; set; }

        public string Audience { get; set; }

        public int AccessExpiration { get; set; }

        public int RefreshExpiration { get; set; }
    }
}
