using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using TRNMNT.Core.Model.User;
using TRNMNT.Core.Services.Interface;
using TRNMNT.Core.Settings;
using TRNMNT.Data.Entities;

namespace TRNMNT.Core.Services.Impl
{
    public class UserService : IUserService
    {
        #region Dependencies

        private readonly UserManager<User> _userManager;
        private readonly ILogger<UserService> _logger;

        #endregion

        #region .ctor

        public UserService(UserManager<User> userManager, ILogger<UserService> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        #endregion

        #region Public Methods

        public async Task<User> GetUserAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<User> GetUserAsync(ClaimsPrincipal claims)
        {
            return await _userManager.GetUserAsync(claims);
        }

        public async Task BackdoorUserCreation(SecretUserCreationModel secretUserCreationModel)
        {
            await ValidateSecretUserCreation(secretUserCreationModel);
            await _userManager.CreateAsync(new User
            {
                UserName = secretUserCreationModel.UserName,
                FirstName = secretUserCreationModel.FirstName,
                LastName = secretUserCreationModel.LastName,
                Email = secretUserCreationModel.Email
            }, secretUserCreationModel.Password);
            
            var createdUser = await _userManager.FindByNameAsync(secretUserCreationModel.UserName);
            await _userManager.AddClaimAsync(createdUser, new Claim(ClaimTypes.Role, Roles.Owner));

            _logger.LogDebug($"User '{secretUserCreationModel.UserName}' is created with role Owner.");
        }

        #endregion

        #region Private Methods

        private async Task ValidateSecretUserCreation(SecretUserCreationModel secretUserCreationModel)
        {
            if (!secretUserCreationModel.AccessKey.Equals("supersecretkeyprostopizdec", StringComparison.CurrentCulture))
            {
                throw new Exception("Unable to create user. Wrong Access Key.");
            }
            var emailUserCheck = await _userManager.FindByEmailAsync(secretUserCreationModel.Email);
            if (emailUserCheck != null)
            {
                throw new Exception("Unable to create user. User with such Email is already exists.");
            }

            var userNameUserCheck = await _userManager.FindByNameAsync(secretUserCreationModel.UserName);
            if (userNameUserCheck != null)
            {
                throw new Exception("Unable to create user. User with such User Name is already exists.");
            }
        }

        #endregion
    }
}
