using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TASRequestForm.Web.Extensions
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

        public static TimeSpan BusinessTimeDelta(this DateTime start, DateTime stop)
        {
            if (start == stop)
                return TimeSpan.Zero;

            if (start > stop)
            {
                DateTime temp = start;
                start = stop;
                stop = temp;
            }

            // First we are going to truncate these DateTimes so that they are within the business day.

            // How much time from the beginning til the end of the day?
            DateTime startFloor = StartOfBusiness(start);
            DateTime startCeil = CloseOfBusiness(start);
            if (start < startFloor) start = startFloor;
            if (start > startCeil) start = startCeil;

            TimeSpan firstDayTime = startCeil - start;
            bool workday = true; // Saves doublechecking later
            if (!IsWorkday(start))
            {
                workday = false;
                firstDayTime = TimeSpan.Zero;
            }

            // How much time from the start of the last day til the end?
            DateTime stopFloor = StartOfBusiness(stop);
            DateTime stopCeil = CloseOfBusiness(stop);
            if (stop < stopFloor) stop = stopFloor;
            if (stop > stopCeil) stop = stopCeil;

            TimeSpan lastDayTime = stop - stopFloor;
            if (!IsWorkday(stop))
                lastDayTime = TimeSpan.Zero;

            // At this point all dates are snipped to within business hours.

            if (start.Date == stop.Date)
            {
                if (!workday) // Precomputed value from earlier
                    return TimeSpan.Zero;

                return stop - start;
            }

            // At this point we know they occur on different dates, so we can use
            // the offset from SOB and COB.

            TimeSpan timeInBetween = TimeSpan.Zero;
            TimeSpan hoursInAWorkday = (startCeil - startFloor);

            // I tried cool math stuff instead of a for-loop, but that leaves no clean way to count holidays.
            for (DateTime itr = startFloor.AddDays(1); itr < stopFloor; itr = itr.AddDays(1))
            {
                if (!IsWorkday(itr))
                    continue;

                // Otherwise, it's a workday!
                timeInBetween += hoursInAWorkday;
            }

            return firstDayTime + lastDayTime + timeInBetween;
        }

        public static bool IsWorkday(DateTime date)
        {
            // Weekend
            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
                return false;

            // Could add holiday logic here.

            return true;
        }

        public static DateTime StartOfBusiness(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
        }

        public static DateTime CloseOfBusiness(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
        }
    }
}