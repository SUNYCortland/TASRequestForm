using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TASRequestForm.Core.Data.Entities;

namespace TASRequestForm.Core.Services
{
    public interface IBannerIdentityService
    {
        BannerIdentity GetBannerIdentityByNetId(string NetId);

        BannerIdentity GetBannerIdentityByPidm(int Pidm);

        BannerIdentity GetBannerIdentityByCNumber(string cNumber);
    }
}
