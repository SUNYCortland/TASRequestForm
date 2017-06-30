using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TASRequestForm.Core.Data.Entities
{
    public class CourseMeeting : Entity<int?>
    {
        public string DaysOfWeek { get; set; }
        public string BeginTime { get; set; }
        public string EndTime { get; set; }
    }
}
