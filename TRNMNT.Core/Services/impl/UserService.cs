using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using TRNMNT.Core.Const;
using TRNMNT.Core.Helpers.Exceptions;
using TRNMNT.Core.Model.Result;
using TRNMNT.Core.Model.User;
using TRNMNT.Core.Services.Interface;
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

        public async Task<string> GetUserRoleAsync(User user){
             return (await _userManager.GetClaimsAsync(user)).FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;
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

        public async Task<UserRegistrationResult> CreateParticipantUserAsync(UserRegistrationModel model)
        {
            return await CreateUserAsync(model, Roles.Participant);
        }

        public async Task UpdateUserAsync(UserModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                throw new BusinessException("ERROR.USER_NOT_FOUND");
            }
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.DateOfBirth = model.DateOfBirth.Value;
            user.Email = model.Email;
            await _userManager.UpdateAsync(user);
        }
        public async Task<UserRegistrationResult> CreateUserAsync(UserRegistrationModel model, string role)
        {
            var user = new User
            {
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                CreateTS = DateTime.UtcNow,
                UserName = model.Email,
                DateOfBirth = model.DateOfBirth.Value,
                IsActive = true
            };
            return await CreateUserWithRoleAsync(user, role, model.Password);
        }

        public async Task ChangesPasswordAsync(string oldPassword, string newPassword, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new BusinessException("ERROR.USER_NOT_FOUND");
            }
            var changeResult = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
            if (!changeResult.Succeeded)
            {
                throw new BusinessException("ERROR.CHANGE_PASSWORD.OLD_PASSWORD_IS_INVALID");
            };
        }

        public async Task SetPasswordAsync(string password, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new BusinessException("ERROR.USER_NOT_FOUND");
            }
            var setPasswordResult = await _userManager.AddPasswordAsync(user, password);
            if (setPasswordResult.Succeeded)
            {
                throw new BusinessException("ERROR.PASSWORD.OLD_PASSWORD_IS_INVALID");
            }
        }

        public async Task<UserRegistrationResult> CreateUserWithRoleAsync(User user, string role, string password)
        {
            var identityResult = await _userManager.CreateAsync(user, password);
            await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, role));

            if (identityResult.Succeeded)
            {
                return new UserRegistrationResult(true);
            }
            return new UserRegistrationResult(false, identityResult.Errors.FirstOrDefault()?.Description);
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