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
    public class NhCourseMeetingRepository : NhRepositoryBase<int?, CourseMeeting>, ICourseMeetingRepository
    {
        private readonly ISession _session;

        public NhCourseMeetingRepository(ISession session)
            : base(session)
        {
            _session = session;
        }

        public CourseMeeting GetCourseMeetingInfoByCRN(string crn)
        {
            var courseMeetingInfo = _session.CreateSQLQuery(GetCourseMeetingInfoTransformer.Sql)
                                    .SetParameter("CRN", crn)
                                    .SetResultTransformer(GetCourseMeetingInfoTransformer.Transformer)
                                    .UniqueResult<CourseMeeting>();

            return courseMeetingInfo;
        }
    }
}
