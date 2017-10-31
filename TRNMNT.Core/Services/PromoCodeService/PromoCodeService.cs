using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TRNMNT.Data.Entities;
using TRNMNT.Data.Repositories;
using TRNMNT.Data.UnitOfWork;

namespace TRNMNT.Core.Services
{
    public class PromoCodeService : IPromoCodeService
    {
        private IRepository<PromoCode> promoCodeRepository;
        private IUnitOfWork unitOfWork;

        public PromoCodeService(IRepository<PromoCode> promoCodeRepository, IUnitOfWork unitOfWork)
        {
            this.promoCodeRepository = promoCodeRepository;
            this.unitOfWork = unitOfWork;
        }


        public async Task<bool> ValidateCodeAsync(Guid eventId, string code, string burntBy)
        {
            var promoCode = await promoCodeRepository.GetAll().Where(pc => pc.IsActive && pc.Code.Equals(code) && pc.EventId == eventId).FirstOrDefaultAsync();
            if (promoCode != null)
            {
                promoCode.BurntBy = burntBy;
                promoCode.IsActive = false;
                promoCode.UpdateTs = DateTime.UtcNow;
                promoCodeRepository.Update(promoCode);
                return true;
            }
            else if (await promoCodeRepository.GetAll().Where(pc => !pc.IsActive && pc.Code.Equals(code) && pc.EventId == eventId && pc.BurntBy.Equals(burntBy)).AnyAsync())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
