using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TASRequestForm.Core.Data.Entities
{
    public class BannerIdentity : Entity<int?>
    {
        public virtual int Pidm { get; set; }
        public virtual string CNumber { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual NetId NetId { get; set; }
        public virtual string Email
        {
            get
            {
                return NetId.Value + "@cortland.edu";
            }
        }
    }
}
