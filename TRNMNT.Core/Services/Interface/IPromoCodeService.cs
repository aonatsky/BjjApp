using System;
using System.Threading.Tasks;

namespace TRNMNT.Core.Services.Interface
{
    public interface IPromoCodeService
    {
        /// <summary>
        /// Burns promocode.
        /// </summary>
        /// <returns>True if code exists and burnt.</returns>
        Task<bool> ValidateCodeAsync(Guid eventId, string promoCode, string burntBy);
    }
}
