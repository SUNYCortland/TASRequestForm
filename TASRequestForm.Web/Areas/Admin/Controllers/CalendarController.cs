using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TASRequestForm.Core.Services;
using TASRequestForm.Web.Areas.Admin.Attributes;
using TASRequestForm.Web.Areas.Admin.Models;

namespace TASRequestForm.Web.Areas.Admin.Controllers
{
    [AdminAuthorize]
    public class CalendarController : Controller
    {
        private readonly ITestTimeService _testTimeService;

        public CalendarController(ITestTimeService testTimeService)
        {
            _testTimeService = testTimeService;
        }

        //
        // GET: /Admin/Calendar/
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetCalendarTimes(DateTime start, DateTime end)
        {
            var testTimes = _testTimeService.GetAllTestTimesBetween(start, end);
            var calendarEvents = new List<CalendarEvent>();

            foreach (var testTime in testTimes)
            {
                string className = String.Empty;

                if (testTime.Canceled)
                    className = "label-default";
                else if (!testTime.FormEntry.ProfessorApprovedDate.HasValue)
                    className = "label-warning";
                else if (!testTime.FormEntry.ApprovedDate.HasValue)
                    className = "label-primary";
                else if (testTime.FormEntry.TASApproved)
                    className = "label-success";
                else if ((!testTime.FormEntry.ProfessorApproved && testTime.FormEntry.ProfessorApprovedDate.HasValue) || (!testTime.FormEntry.TASApproved && testTime.FormEntry.ApprovedDate.HasValue))
                    className = "label-default";

                var calendarEvent = new CalendarEvent
                {
                    id = testTime.FormEntry.Id.Value,
                    title = testTime.FormEntry.StudentIdentity.LastName + ", " + testTime.FormEntry.StudentIdentity.FirstName + " (" + testTime.FormEntry.StudentIdentity.CNumber +") - " + testTime.FormEntry.Course,
                    start = testTime.Date.ToString("s", System.Globalization.CultureInfo.InvariantCulture),
                    url = Url.Action("View", new { Controller = "Requests", id = testTime.FormEntry.Id.Value, area = "Admin" }),
                    className = className
                };

                calendarEvents.Add(calendarEvent);
            }

            return Json(calendarEvents, JsonRequestBehavior.AllowGet);
        }
	}
}