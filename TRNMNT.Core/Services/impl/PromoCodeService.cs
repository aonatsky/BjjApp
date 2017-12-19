using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TRNMNT.Core.Services.Interface;
using TRNMNT.Data.Entities;
using TRNMNT.Data.Repositories;

namespace TRNMNT.Core.Services.Impl
{
    public class PromoCodeService : IPromoCodeService
    {
        #region Dependencies

        private readonly IRepository<PromoCode> _promoCodeRepository;

        #endregion

        #region .ctor

        public PromoCodeService(IRepository<PromoCode> promoCodeRepository)
        {
            _promoCodeRepository = promoCodeRepository;
        }

        #endregion

        #region Public Methods

        public async Task<bool> ValidateCodeAsync(Guid eventId, string code, string burntBy)
        {
            var promoCode = await _promoCodeRepository.GetAll().Where(pc => pc.IsActive && pc.Code.Equals(code) && pc.EventId == eventId).FirstOrDefaultAsync();
            if (promoCode != null)
            {
                promoCode.BurntBy = burntBy;
                promoCode.IsActive = false;
                promoCode.UpdateTs = DateTime.UtcNow;
                _promoCodeRepository.Update(promoCode);
                return true;
            }
            return await _promoCodeRepository.GetAll().Where(pc => !pc.IsActive && pc.Code.Equals(code) && pc.EventId == eventId && pc.BurntBy.Equals(burntBy)).AnyAsync();
        }

        #endregion
    }
}
