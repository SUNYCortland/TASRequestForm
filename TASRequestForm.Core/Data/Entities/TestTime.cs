using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TASRequestForm.Core.Data.Entities
{
    public class TestTime : Entity<int?>
    {
        public virtual DateTime Date { get; set; }
        public virtual string Type { get; set; }
        public virtual string Title { get; set; }
        public virtual bool NoShow { get; set; }
        public virtual bool Canceled { get; set; }
        public virtual int? CanceledByPidm { get; set; }
        public virtual DateTime? CanceledDate { get; set; }
        public virtual FormEntry FormEntry { get; set; }

        public virtual BannerIdentity CanceledByIdentity { get; set; }
    }
}
