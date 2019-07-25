using Drypoint.Host.Core.Authentication.JwtBearer;
using Drypoint.Unity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drypoint.Host.Core.Authentication
{
    public static class AuthConfigurer
    {
        public static void AddAuthenConfigure(this AuthenticationBuilder authenticationBuilder, IConfiguration configuration)
        {
            if (bool.Parse(configuration["Authentication:JwtBearer:IsEnabled"]))
            {
                authenticationBuilder.AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        // The signing key must match!
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey =
                            new SymmetricSecurityKey(
                                Encoding.ASCII.GetBytes(configuration["Authentication:JwtBearer:SecurityKey"])),

                        // Validate the JWT Issuer (iss) claim
                        ValidateIssuer = true,
                        ValidIssuer = configuration["Authentication:JwtBearer:Issuer"],

                        // Validate the JWT Audience (aud) claim
                        ValidateAudience = true,
                        ValidAudience = configuration["Authentication:JwtBearer:Audience"],

                        // Validate the token expiry
                        ValidateLifetime = true,

                        // If you want to allow a certain amount of clock drift, set that here
                        ClockSkew = TimeSpan.Zero
                    };

                    options.SecurityTokenValidators.Clear();
                    options.SecurityTokenValidators.Add(new JwtSecurityTokenValidator());

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = QueryStringTokenResolver
                    };
                });
            }
            if (bool.Parse(configuration["IdentityServer:IsEnabled"] ?? "false"))
            {
                IdentityModelEventSource.ShowPII = true;
                authenticationBuilder.AddIdentityServerAuthentication("IdentityBearer", options =>
                {
                    options.Authority = configuration["IdentityServer:Authority"];
                    options.ApiName = configuration["IdentityServer:ApiName"];
                    options.ApiSecret = configuration["IdentityServer:ApiSecret"];
                    options.RequireHttpsMetadata = false;
                });
            }
        }

        /* 此方法用于授权SignalR javascript客户机。
         * SignalR无法发送授权头。因此，我们将它作为加密文本从查询字符串中获取。 */
        private static Task QueryStringTokenResolver(MessageReceivedContext context)
        {
            if (!context.HttpContext.Request.Path.HasValue ||
                !context.HttpContext.Request.Path.Value.StartsWith("/signalr"))
                //We are just looking for signalr clients
                return Task.CompletedTask;

            var qsAuthToken = context.HttpContext.Request.Query["enc_auth_token"].FirstOrDefault();
            if (qsAuthToken == null)
            {
                return Task.CompletedTask;
            }
            context.Token = SimpleStringCipher.Instance.Decrypt(qsAuthToken, DrypointConsts.DefaultPassPhrase);
            return Task.CompletedTask;
        }
    }
}
