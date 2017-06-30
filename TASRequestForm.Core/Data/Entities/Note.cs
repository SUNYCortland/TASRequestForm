using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TASRequestForm.Core.Data.Entities
{
    public class Note : Entity<int?>
    {
        public virtual string Value { get; set; }
        public virtual FormEntry FormEntry { get; set; }
        public virtual int AddedByPidm { get; set; }
        public virtual BannerIdentity AddedByIdentity { get; set; }
        public virtual string Type { get; set; }
        public virtual DateTime AddedDate { get; set; }
    }
}
