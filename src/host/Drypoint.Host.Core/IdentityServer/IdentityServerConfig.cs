using System.Collections.Generic;
using System.Linq;
using IdentityServer4;
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
        public static IEnumerable<IdentityServer4.Models.ApiResource> GetApiResources(IConfigurationRoot configuration)
        {
            return new List<ApiResource>
            {
                new ApiResource()
                {
                    //希望保护的API
                    Name=configuration["IdentityServer:ApiName"],
                    DisplayName="Default (all) API",
                    Description = "All API",
                    ApiSecrets= {new Secret(configuration["IdentityServer:ApiSecret"].Sha256()) },
                    //请求范围
                    //Scopes = new List<Scope> {
                    //    new Scope("api.read"),
                    //    new Scope("api.write")
                    //}
                }
            };
        }

        /// <summary>
        /// 身份资源范围
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<IdentityResource> GetIdentityResources(IConfigurationRoot configuration)
        {
            //IdentityServer支持的一些标准OpenID Connect定义的范围
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),  //必须
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new IdentityResources.Phone(),
                //自定义
                //new IdentityResource {
                //    Name = "role",
                //    UserClaims = new List<string> {"role"}
                //}
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
                    AllowedCorsOrigins= child.GetSection("AllowedCorsOrigins").GetChildren().Select(c => c.Value)?.ToArray(),
                    AccessTokenType = AccessTokenType.Jwt,
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

        public static List<IdentityServer4.Test.TestUser> GetTestUser() {
            List<IdentityServer4.Test.TestUser> ltUser = new List<IdentityServer4.Test.TestUser>();

            ltUser.Add(new IdentityServer4.Test.TestUser {
                SubjectId="1",
                Username = "admin",
                Password = "123456"
            });
            ltUser.Add(new IdentityServer4.Test.TestUser
            {
                SubjectId="2",
                Username = "user",
                Password = "123456"
            });
            return ltUser;
        }
    }
}
