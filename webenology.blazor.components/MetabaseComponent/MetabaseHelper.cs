using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using AngleSharp.Common;
using HtmlAgilityPack;
using Microsoft.IdentityModel.Tokens;
using Spire.Doc.Documents;

namespace webenology.blazor.components;

public interface IMetabaseHelper
{
    string GenerateJwt(int resourceId, string secretKey, bool isDashboard, Dictionary<string, string>? jwtParameters,
        string oldJwt);
}

public class MetabaseHelper : IMetabaseHelper
{
    private readonly string? _metabaseSecretKey;

    public MetabaseHelper(string? metabaseSecretKey)
    {
        _metabaseSecretKey = metabaseSecretKey;
    }

    public string GenerateJwt(int resourceId, string secretKey, bool isDashboard,
        Dictionary<string, string>? jwtParameters, string oldJwt)
    {
        var handler = new JwtSecurityTokenHandler();
        if (!string.IsNullOrEmpty(oldJwt))
        {
            var token = handler.ReadJwtToken(oldJwt);
            var jwtParams = token.Payload["params"];
            var sameParams = true;
            if (jwtParameters != null)
            {
                var dict = jwtParams.ToDictionary();
                foreach (var key in jwtParameters.Keys)
                {
                    if (dict.ContainsKey(key))
                    {
                        if (dict[key] == jwtParameters[key])
                            continue;
                    }

                    sameParams = false;
                }
            }

            if (sameParams && token.ValidTo > DateTime.Now.AddMinutes(3))
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
        if (jwtParameters != null)
        {
            foreach (var jwt in jwtParameters)
            {
                pars.Add(jwt.Key, jwt.Value);
            }
        }

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