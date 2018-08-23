using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TRNMNT.Core.Const;
using TRNMNT.Core.Helpers.Exceptions;
using TRNMNT.Core.Model.Result;
using TRNMNT.Core.Model.User;
using TRNMNT.Core.Services.Interface;
using TRNMNT.Data.Entities;
using TRNMNT.Data.Repositories;

namespace TRNMNT.Core.Services.Impl
{
    public class UserService : IUserService
    {
        #region Dependencies

        private readonly UserManager<User> _userManager;
        private readonly ILogger<UserService> _logger;
        private readonly IRepository<User> _repository;

        #endregion

        #region .ctor

        public UserService(UserManager<User> userManager, ILogger<UserService> logger, IRepository<User> repository)
        {
            _userManager = userManager;
            _logger = logger;
            _repository = repository;
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

        #region Public Methods

        public async Task<User> GetUserAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<IEnumerable<User>> GetUsersAsync(Expression<Func<User, bool>> predicate)
        {
            return await _repository.GetAll(predicate).ToListAsync();
        }

        public async Task<List<string>> GetUserRolesAsync(User user)
        {
            return (await _userManager.GetClaimsAsync(user)).Where(c => c.Type == ClaimTypes.Role).Select(r => r.Value).ToList();
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

        public async Task CreateParticipantUserAsync(UserModelRegistration model)
        {
            var roles = new List<string> { Roles.Participant };
            if (model.IsTeamOwner)
            {
                roles.Add(Roles.TeamOwner);
            }
            await CreateUserAsync(model, roles);
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
            user.TeamId = model.TeamId;
            await _userManager.UpdateAsync(user);
        }
        public async Task CreateUserAsync(UserModelRegistration model, List<string> roles)
        {
            var user = new User
            {
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                CreateTS = DateTime.UtcNow,
                UserName = model.Email,
                DateOfBirth = model.DateOfBirth.Value,
                IsActive = true,
                TeamId = model.TeamId,
                TeamMembershipApprovalStatus = ApprovalStatus.Pending
            };
            await CreateUserWithRoleAsync(user, roles, model.Password);
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

        public async Task CreateUserWithRoleAsync(User user, List<string> roles, string password)
        {
            var identityResult = await _userManager.CreateAsync(user, password);
            foreach (string role in roles)
            {
                await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, role));
            }
            if (!identityResult.Succeeded)
            {
                if(identityResult.Errors.Any(e => e.Code == "DuplicateUserName"))
                {
                    throw new BusinessException("ERROR.DUPLICATE_EMAIL");
                }
                throw new BusinessException("ERROR.REGISTRATION_FAILED");
            }
        }

        public async Task DeclineTeamMembershipAsync(Guid teamId, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null || user.TeamId != teamId)
            {
                return;
            }
            user.TeamMembershipApprovalStatus = ApprovalStatus.Pending;
            user.TeamId = null;
            await _userManager.UpdateAsync(user);
        }

        public async Task ApproveTeamMembershipAsync(Guid teamId, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null || user.TeamId != teamId)
            {
                return;
            }
            user.TeamMembershipApprovalStatus = ApprovalStatus.Approved;
            await _userManager.UpdateAsync(user);
        }

        public async Task SetTeamForUserAsync(Guid teamId, string userId, bool isOwner = false)
        {
            var user = await _userManager.FindByIdAsync(userId);
            user.TeamId = teamId;
            user.TeamMembershipApprovalStatus = ApprovalStatus.Pending;
            if (isOwner)
            {
                user.TeamMembershipApprovalStatus = ApprovalStatus.Approved;
            }
            await _userManager.UpdateAsync(user);
        }

        #endregion

    }
}