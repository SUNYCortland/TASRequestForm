using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TASRequestForm.Core.Data.Entities;
using TASRequestForm.Core.Data.Repositories;

namespace TASRequestForm.Core.Services.Impl
{
    public class CourseMeetingService : ICourseMeetingService
    {
        private readonly ICourseMeetingRepository _courseMeetingRepository;

        public CourseMeetingService(ICourseMeetingRepository courseMeetingRepository)
        {
            _courseMeetingRepository = courseMeetingRepository;
        }

        public CourseMeeting GetCourseMeetingInfoByCRN(string crn)
        {
            return _courseMeetingRepository.GetCourseMeetingInfoByCRN(crn);
        }
    }
}
