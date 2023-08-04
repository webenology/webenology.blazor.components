using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace webenology.blazor.components.MetabaseComponent;

public class MetabaseHelper
{
    public string GetJWT(int resourceId, string secretKey, bool isDashboard)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials =
            new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
        var header = new JwtHeader(credentials);

        var resParam = new Dictionary<string, int>();
        resParam.Add((isDashboard?"dashboard":"question"), resourceId);
        
        //Empty dictionary for the params. Anything else gives odd results
        var pars = new Dictionary<string, string>();
        
        JwtPayload payload = new JwtPayload
        {
            { "resource", resParam },
            { "params", pars }
        };

        var secToken = new JwtSecurityToken(header, payload);
        var handler = new JwtSecurityTokenHandler();
        var tokenString = handler.WriteToken(secToken);
        return tokenString;
    }
}