using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TASRequestForm.Core.Data.Entities;
using TASRequestForm.Core.Data.Repositories;

namespace TASRequestForm.Core.Services.Impl
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;

        public CourseService(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public IEnumerable<Course> GetCoursesByPidm(int pidm)
        {
            return _courseRepository.GetCoursesByPidm(pidm);
        }
    }
}
