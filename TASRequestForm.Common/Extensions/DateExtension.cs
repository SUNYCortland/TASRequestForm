using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TASRequestForm.Common.Extensions
{
    public static class DateExtensions
    {
        public static bool IsBusinessDay(this DateTime date)
        {
            return
                date.DayOfWeek != DayOfWeek.Saturday &&
                date.DayOfWeek != DayOfWeek.Sunday;
        }
        public static int BusinessDaysTo(this DateTime fromDate, DateTime toDate,
                                         int maxAllowed = 0)
        {
            int ret = 0;
            DateTime dt = fromDate;
            while (dt < toDate)
            {
                if (dt.IsBusinessDay()) ret++;
                if (maxAllowed > 0 && ret == maxAllowed) return ret;
                dt = dt.AddDays(1);
            }
            return ret;
        }
    }
}
