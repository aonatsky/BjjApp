﻿using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using TRNMNT.Core.Services.Interface;
using TRNMNT.Data.Entities;

namespace TRNMNT.Core.Services.Impl
{
    public class UserService : IUserService
    {
        private UserManager<User> userManager;

        public UserService(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<User> GetUserAsync(string userId)
        {
            return await userManager.FindByIdAsync(userId);
        }

        public async Task<User> GetUserAsync(ClaimsPrincipal claims )
        {
            return await userManager.GetUserAsync(claims);
        }
    }
}
