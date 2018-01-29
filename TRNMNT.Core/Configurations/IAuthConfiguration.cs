using Microsoft.IdentityModel.Tokens;

namespace TRNMNT.Core.Configurations
{
    public interface IAuthConfiguration
    {
        SymmetricSecurityKey Key { get; }
        string Audience { get; }
        string Issuer { get; }
        int AccessTokenLifetime { get; }
        int RefreshTokenLifetime { get; }
    }
}
