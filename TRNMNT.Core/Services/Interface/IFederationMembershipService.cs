using System;
using System.Threading.Tasks;

namespace TRNMNT.Core.Services.Interface
{
    public interface IFederationMembershipService
    {
        Task<bool> IsFederationMemberAsync(Guid federationId, string userId);
    }
}
