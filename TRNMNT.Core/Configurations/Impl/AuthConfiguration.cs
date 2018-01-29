using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace TRNMNT.Core.Configurations.Impl
{
    public class AuthConfiguration : IAuthConfiguration
    {
        #region Dependencies

        private readonly IConfiguration _authConfiguration;

        #endregion

        #region .ctor

        public AuthConfiguration(IConfiguration configuration)
        {
            _authConfiguration = configuration.GetSection("Auth");
        }

        #endregion

        #region Properties

        public SymmetricSecurityKey Key => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authConfiguration.GetSection(nameof(Key)).Value));
        public string Audience => _authConfiguration.GetSection(nameof(Audience)).Value;
        public string Issuer => _authConfiguration.GetSection(nameof(Issuer)).Value;
        public int AccessTokenLifetime => int.Parse(_authConfiguration.GetSection(nameof(AccessTokenLifetime)).Value);
        public int RefreshTokenLifetime => int.Parse(_authConfiguration.GetSection(nameof(RefreshTokenLifetime)).Value);

        #endregion
    }
}