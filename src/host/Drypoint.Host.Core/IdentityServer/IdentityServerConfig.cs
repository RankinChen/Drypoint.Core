using System.Collections.Generic;
using System.Linq;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;

namespace Drypoint.Host.Core.IdentityServer
{
    public static class IdentityServerConfig
    {
        /// <summary>
        /// api资源
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<IdentityServer4.Models.ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource()
                {
                    //希望保护的API
                    Name="api",
                    DisplayName="Default (all) API",
                    Description = "All API",
                    ApiSecrets= {new Secret("secret".Sha256()) },
                    //请求范围
                    Scopes = new List<Scope> {
                        new Scope("api.read"),
                        new Scope("api.write")
                    }
                }
            };
        }

        /// <summary>
        /// 身份资源范围
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            //IdentityServer支持的一些标准OpenID Connect定义的范围
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),  //必须
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new IdentityResources.Phone(),
                //自定义
                new IdentityResource {
                    Name = "role",
                    UserClaims = new List<string> {"role"}
                }
            };
        }

        /// <summary>
        /// 客户端
        /// </summary>
        public static IEnumerable<Client> GetClients(IConfigurationRoot configuration)
        {
            var clients = new List<Client>();

            foreach (var child in configuration.GetSection("IdentityServer:Clients").GetChildren())
            {
                clients.Add(new Client
                {
                    ClientId = child["ClientId"],
                    ClientName = child["ClientName"],
                    AllowedGrantTypes = child.GetSection("AllowedGrantTypes").GetChildren().Select(c => c.Value).ToArray(),
                    RequireConsent = bool.Parse(child["RequireConsent"] ?? "false"),
                    AllowOfflineAccess = bool.Parse(child["AllowOfflineAccess"] ?? "false"),
                    ClientSecrets = child.GetSection("ClientSecrets").GetChildren().Select(secret => new Secret(secret["Value"].Sha256())).ToArray(),
                    AllowedScopes = child.GetSection("AllowedScopes").GetChildren().Select(c => c.Value).ToArray(),
                    RedirectUris = child.GetSection("RedirectUris").GetChildren().Select(c => c.Value).ToArray(),
                    PostLogoutRedirectUris = child.GetSection("PostLogoutRedirectUris").GetChildren().Select(c => c.Value).ToArray(),
                });
            }

            return clients;
        }
    }
}
