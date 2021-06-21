using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Drypoint.Core.Extensions.Authentication.JwtBearer
{
    public class JwtSecurityTokenValidator : ISecurityTokenValidator
    {
        private readonly JwtSecurityTokenHandler _tokenHandler;
        public JwtSecurityTokenValidator()
        {
            _tokenHandler = new JwtSecurityTokenHandler();
        }

        public bool CanValidateToken => true;

        public int MaximumTokenSizeInBytes { get; set; } = TokenValidationParameters.DefaultMaximumTokenSizeInBytes;

        public bool CanReadToken(string securityToken)
        {
            return _tokenHandler.CanReadToken(securityToken);
        }

        //验证Token 程序逻辑验证是否失效
        public ClaimsPrincipal ValidateToken(string securityToken, TokenValidationParameters validationParameters,
            out SecurityToken validatedToken)
        {
            var principal = _tokenHandler.ValidateToken(securityToken, validationParameters, out validatedToken);

            return principal;
            //var tokenValidityKey = principal.Claims.First(c => c.Type == JwtClaimTypes.Id);

            //var userIdentifierString = await _memoryCache.GetOrCreateAsync<string>(tokenValidityKey, factory =>
            //  {
            //      factory.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(3); //3秒后释放
            //    return factory;
            //  });

            //_memoryCache.TryGetValue(tokenValidityKey, out object userIdentifierString);

            //if (userIdentifierString != null)
            //{
            //    return principal;
            //}

            //if (long.TryParse(userIdentifierString.Value, out long userIdentifier))
            //{
            //    var isValidityKetValid = true; //验证缓存的Token是否过期

            //    if (isValidityKetValid)
            //    {
            //        _memoryCache.GetOrCreate(DrypointConsts.CacheKey_TokenValidityKey, factory =>
            //        {
            //            //过期
            //            return "";
            //        }); ;

            //        return principal;
            //    }
            //}
            //throw new SecurityTokenException("invalid");
        }
    }
}
