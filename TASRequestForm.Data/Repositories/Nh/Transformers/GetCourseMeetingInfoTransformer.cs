using NHibernate.Transform;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TASRequestForm.Core.Data.Entities;

namespace TASRequestForm.Data.Repositories.Nh.Transformers
{
    public class GetCourseMeetingInfoTransformer : IResultTransformer
    {
        public static readonly string Sql =
            @"SELECT
                SSRMEET_MON_DAY mon,
                SSRMEET_TUE_DAY tue,
                SSRMEET_WED_DAY wed,
                SSRMEET_THU_DAY thu,
                SSRMEET_FRI_DAY fri,
                SSRMEET_SAT_DAY sat,
                SSRMEET_SUN_DAY sun,
                SSRMEET_BEGIN_TIME btime,
                SSRMEET_END_TIME etime
            FROM
                SSRMEET
            WHERE SSRMEET_TERM_CODE = (SELECT cortfunct.f_get_current_term() as currentTerm FROM dual)
            AND SSRMEET_CRN = :CRN";

        public static readonly GetCourseMeetingInfoTransformer Transformer = new GetCourseMeetingInfoTransformer();

        private GetCourseMeetingInfoTransformer() { }

        public IList TransformList(IList collection)
        {
            return collection;
        }

        public object TransformTuple(object[] tuple, string[] aliases)
        {
            DateTime beginTime = new DateTime();
            DateTime endTime = new DateTime();
            var daysOfWeek = new List<string>();

            if (tuple[0] != null)
                daysOfWeek.Add(tuple[0].ToString());

            if (tuple[1] != null)
                daysOfWeek.Add(tuple[1].ToString());

            if (tuple[2] != null)
                daysOfWeek.Add(tuple[2].ToString());

            if (tuple[3] != null)
                daysOfWeek.Add(tuple[3].ToString());

            if (tuple[4] != null)
                daysOfWeek.Add(tuple[4].ToString());

            if (tuple[5] != null)
                daysOfWeek.Add(tuple[5].ToString());

            if (tuple[6] != null)
                daysOfWeek.Add(tuple[6].ToString());

            if (tuple[7] != null)
                beginTime = DateTime.ParseExact(tuple[7].ToString(), "HHmm", System.Globalization.CultureInfo.InvariantCulture);

            if (tuple[8] != null)
                endTime = DateTime.ParseExact(tuple[8].ToString(), "HHmm", System.Globalization.CultureInfo.InvariantCulture);

            return new CourseMeeting
            {
                BeginTime = beginTime.ToString("h:mm tt "),
                EndTime = endTime.ToString("h:mm tt "),
                DaysOfWeek = string.Join(",", daysOfWeek)
            };
        }
    }
}
