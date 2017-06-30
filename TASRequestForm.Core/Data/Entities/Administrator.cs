using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TASRequestForm.Core.Data.Entities
{
    public class Administrator : Entity<int?>
    {
        public virtual int Pidm { get; set; }
        public virtual BannerIdentity AdministratorIdentity { get; set; }
    }
}
