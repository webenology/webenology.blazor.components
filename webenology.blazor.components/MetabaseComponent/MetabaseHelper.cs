using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using HtmlAgilityPack;
using Microsoft.IdentityModel.Tokens;

namespace webenology.blazor.components;

public interface IMetabaseHelper
{
    string GenerateJwt(int resourceId, string secretKey, bool isDashboard, string oldJwt);
}

public class MetabaseHelper : IMetabaseHelper
{
    private readonly string? _metabaseSecretKey;

    public MetabaseHelper(string? metabaseSecretKey)
    {
        _metabaseSecretKey = metabaseSecretKey;
    }

    public string GenerateJwt(int resourceId, string? secretKey, bool isDashboard, string oldJwt)
    {
        var handler = new JwtSecurityTokenHandler();
        if (!string.IsNullOrEmpty(oldJwt))
        {
            var token = handler.ReadJwtToken(oldJwt);
            if (token.ValidTo > DateTime.Now.AddMinutes(3))
                return oldJwt;
        }

        secretKey = !string.IsNullOrEmpty(_metabaseSecretKey) ? _metabaseSecretKey : secretKey;
        if (string.IsNullOrEmpty(secretKey))
            return string.Empty;

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials =
            new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
        var header = new JwtHeader(credentials);

        var resParam = new Dictionary<string, int> { { isDashboard ? "dashboard" : "question", resourceId } };

        var pars = new Dictionary<string, string>();
        var expiration = DateTime.Now.AddMinutes(10);

        var payload = new JwtPayload
        {
            { "resource", resParam },
            { "params", pars },
            { "exp", expiration.Ticks }
        };

        var secToken = new JwtSecurityToken(header, payload);
        var tokenString = handler.WriteToken(secToken);
        return tokenString;
    }
}

public enum MetabaseTheme
{
    Light,
    Dark,
    Transparent
}