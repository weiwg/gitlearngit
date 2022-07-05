
using System;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using LY.Report.Core.Common.Configs;
using LY.Report.Core.Common.Attributes;
using System.Linq;
using LY.Report.Core.Common.Extensions;

namespace LY.Report.Core.Common.Auth
{
    [SingleInstance]
    public class UserToken : IUserToken
    {
        private readonly JwtConfig _jwtConfig;

        public UserToken(JwtConfig jwtConfig)
        {
            _jwtConfig = jwtConfig;
        }

        public string Create(Claim[] claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.SecurityKey));
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var timestamp = DateTime.Now.AddMinutes(_jwtConfig.Expires + _jwtConfig.RefreshExpires).ToTimestamp().ToString();
             claims = claims.Append(new Claim(ClaimAttributes.RefreshExpires, timestamp)).ToArray();

            var token = new JwtSecurityToken(
                issuer: _jwtConfig.Issuer,
                audience: _jwtConfig.Audience,
                claims: claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddMinutes(_jwtConfig.Expires),
                signingCredentials: signingCredentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public Claim[] Decode(string jwtToken)
        {
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = jwtSecurityTokenHandler.ReadJwtToken(jwtToken);
            return jwtSecurityToken?.Claims?.ToArray();
        }

        public bool Validate(string jwtToken)
        {
            if (jwtToken.IsNull())
            {
                return false;
            }
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var tokenParams = new TokenValidationParameters
            {
                ValidateIssuer = true,//是否验证发行人，就是验证载荷中的Iss是否对应ValidIssuer参数
                ValidateAudience = true,//是否验证订阅人，就是验证载荷中的Aud是否对应ValidAudience参数
                ValidateLifetime = true,//是否验证过期时间，过期了就拒绝访问
                ValidateIssuerSigningKey = true,//是否验证签名,不验证的话可以篡改数据，不安全
                ValidIssuer = _jwtConfig.Issuer,//发行人
                ValidAudience = _jwtConfig.Audience,//订阅人
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.SecurityKey)),//解密的密钥
                ClockSkew = TimeSpan.Zero//这个是缓冲过期时间，也就是说，即使我们配置了过期时间，这里也要考虑进去，过期时间+缓冲，默认好像是7分钟，你可以直接设置为0
            };
            SecurityToken validatedToken;
            try
            {
                jwtSecurityTokenHandler.ValidateToken(jwtToken, tokenParams, out validatedToken);
            }
            catch (Exception)
            {
                return false;
            }
            return validatedToken != null;
        }

        public bool ValidateWithoutTime(string jwtToken, out Claim[] claims)
        {
            if (jwtToken.IsNull())
            {
                claims = new Claim[0];
                return false;
            }
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = jwtSecurityTokenHandler.ReadJwtToken(jwtToken);
            claims = jwtSecurityToken?.Claims?.ToArray();
            if (claims == null || claims.Length == 0)
            {
                return false;
            }
            var tokenParams = new TokenValidationParameters
            {
                ValidateIssuer = true,//是否验证发行人，就是验证载荷中的Iss是否对应ValidIssuer参数
                ValidateAudience = true,//是否验证订阅人，就是验证载荷中的Aud是否对应ValidAudience参数
                ValidateLifetime = false,//是否验证过期时间，过期了就拒绝访问
                ValidateIssuerSigningKey = true,//是否验证签名,不验证的话可以篡改数据，不安全
                ValidIssuer = _jwtConfig.Issuer,//发行人
                ValidAudience = _jwtConfig.Audience,//订阅人
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.SecurityKey)),//解密的密钥
                ClockSkew = TimeSpan.Zero//这个是缓冲过期时间，也就是说，即使我们配置了过期时间，这里也要考虑进去，过期时间+缓冲，默认好像是7分钟，你可以直接设置为0
            };
            SecurityToken validatedToken;
            try
            {
                jwtSecurityTokenHandler.ValidateToken(jwtToken, tokenParams, out validatedToken);
            }
            catch (Exception)
            {
                return false;
            }
            return validatedToken != null;
        }
    }
}
