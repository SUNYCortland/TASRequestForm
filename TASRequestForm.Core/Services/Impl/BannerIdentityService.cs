using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TASRequestForm.Core.Data.Entities;
using TASRequestForm.Core.Data.Repositories;

namespace TASRequestForm.Core.Services.Impl
{
    public class BannerIdentityService : IBannerIdentityService
    {
        private readonly IBannerIdentityRepository _bannerIdentityRepository;

        public BannerIdentityService(IBannerIdentityRepository bannerIdentityRepository)
        {
            _bannerIdentityRepository = bannerIdentityRepository;
        }

        public BannerIdentity GetBannerIdentityByNetId(string NetId)
        {
            return _bannerIdentityRepository.FindBy(x => x.NetId.Value.ToUpper() == NetId.ToUpper());
        }

        public BannerIdentity GetBannerIdentityByPidm(int Pidm)
        {
            return _bannerIdentityRepository.FindBy(x => x.Pidm == Pidm);
        }

        public BannerIdentity GetBannerIdentityByCNumber(string cNumber)
        {
            return _bannerIdentityRepository.FindBy(x => x.CNumber.ToUpper() == cNumber.ToUpper());
        }
    }
}
