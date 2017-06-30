using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TASRequestForm.Core.Data.Entities
{
    public class NetId : Entity<int?>
    {
        public virtual int Pidm { get; set; }
        public virtual string Value { get; set; }

        public override string ToString()
        {
            return this.Value;
        }
    }
}
