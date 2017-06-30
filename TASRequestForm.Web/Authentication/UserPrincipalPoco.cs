using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TASRequestForm.Core.Data.Entities;

namespace TASRequestForm.Web.Authentication
{
    public class UserPrincipalPoco
    {
        public bool IsAdmin { get; set; }
        public BannerIdentity BannerIdentity { get; set; }
    }
}