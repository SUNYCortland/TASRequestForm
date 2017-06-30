using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using TASRequestForm.Core.Data.Entities;

namespace TASRequestForm.Web.Authentication
{
    public interface IUserPrincipal : IPrincipal
    {
        BannerIdentity BannerIdentity { get; set; }
        bool IsAdmin { get; set; }
    }
}
