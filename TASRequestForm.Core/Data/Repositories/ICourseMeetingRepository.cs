using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TASRequestForm.Core.Data.Entities;

namespace TASRequestForm.Core.Data.Repositories
{
    public interface ICourseMeetingRepository
    {
        CourseMeeting GetCourseMeetingInfoByCRN(string crn);
    }
}
