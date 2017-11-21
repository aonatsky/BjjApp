using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace TRNMNT.Web.Core.Settings
{
    public static class TokenAuthOptions
    {
        public static SecurityKey GetKey()
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes("supersecretkeyprostopizdec"));
        }
        public const string Audience = "trnmntAudience";
        public const string Issuer = "trnmntIssuer";
        public const int Lifetime = 50; 

    }
}
