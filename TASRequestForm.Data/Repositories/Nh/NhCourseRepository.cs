using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TASRequestForm.Core.Data.Entities;
using TASRequestForm.Core.Data.Repositories;
using TASRequestForm.Data.Repositories.Nh.Transformers;

namespace TASRequestForm.Data.Repositories.Nh
{
    public class NhCourseRepository : NhRepositoryBase<int?, Course>, ICourseRepository
    {
        private readonly ISession _session;

        public NhCourseRepository(ISession session)
            : base(session)
        {
            _session = session;
        }

        public IEnumerable<Course> GetCoursesByPidm(int pidm)
        {
            var courses = _session.CreateSQLQuery(GetCoursesTransformer.Sql)
                                    .SetParameter("Pidm", pidm)
                                    .SetResultTransformer(GetCoursesTransformer.Transformer)
                                    .List<Course>();

            return courses;
        }
    }
}
