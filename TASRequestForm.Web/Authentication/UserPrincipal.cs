using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using TASRequestForm.Core.Data.Entities;

namespace TASRequestForm.Web.Authentication
{
    public class UserPrincipal : IUserPrincipal
    {
        public bool IsAdmin { get; set; }
        public BannerIdentity BannerIdentity { get; set; }
        public IIdentity Identity { get; private set; }

        public UserPrincipal(string Username)
        {
            this.Identity = new GenericIdentity(Username);
        }

        public bool IsInRole(string role)
        {
            throw new NotImplementedException();
        }
    }
}