using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using TASRequestForm.Core.Data.Entities;
using TASRequestForm.Core.Services;
using TASRequestForm.Web.Authentication;
using TASRequestForm.Web.ViewModels;

namespace TASRequestForm.Web.Controllers
{
    [Authorize]
    public class RequestController : Controller
    {
        private bool DEBUGMODE = bool.Parse(WebConfigurationManager.AppSettings["DebugMode"]);

        private readonly ICourseService _courseService;
        private readonly ICourseMeetingService _courseMeetingService;
        private readonly IFormEntryService _formEntryService;
        private readonly IEmailService _emailService;
        private readonly IBannerIdentityService _bannerIdentityService;
        private readonly ITestTimeService _testTimeService;

        public RequestController(ICourseService courseService, 
                                ICourseMeetingService courseMeetingService, 
                                IFormEntryService formEntryService, 
                                IEmailService emailService,
                                IBannerIdentityService bannerIdentityService,
                                ITestTimeService testTimeService)
        {
            _courseService = courseService;
            _courseMeetingService = courseMeetingService;
            _formEntryService = formEntryService;
            _emailService = emailService;
            _bannerIdentityService = bannerIdentityService;
            _testTimeService = testTimeService;
        }

        public ActionResult Index()
        {
            var user = User as UserPrincipal;
            var vm = new RequestIndexViewModel();

            var courses = new List<Course>();

            if (DEBUGMODE)
                courses = _courseService.GetCoursesByPidm(14277).ToList();
            else
                courses = _courseService.GetCoursesByPidm(user.BannerIdentity.Pidm).ToList();

            vm.Courses = courses.Select(x => 
                new SelectListItem { 
                    Text = x.Subject + " " + x.Number + " - " + x.Title,
                    Value = x.CRN + "|" + x.Subject + " " + x.Number + " - " + x.Title + "|" + x.ProfessorPidm
                });

            // Get the latest request to pull in data
            vm.PreviousFormEntry = _formEntryService.GetAllStudentFormEntriesByPidm(user.BannerIdentity.Pidm)
                                        .Where(x => x.ProfessorApprovedDate.HasValue)
                                        .OrderByDescending(x => x.ProfessorApprovedDate)
                                        .FirstOrDefault();

            return View(vm);
        }

        [HttpPost]
        public ActionResult Index(RequestIndexViewModel vm)
        {
            var user = User as UserPrincipal;

            if (ModelState.IsValid)
            {
                var courseArr = vm.SelectedCourse.Split('|');

                // Add student pidm
                vm.FormEntry.StudentPidm = user.BannerIdentity.Pidm;

                // Add professor pidm from course selected
                int professorPidm = int.Parse(courseArr[2]);
                vm.FormEntry.ProfessorPidm = professorPidm;

                // Add course info from selected course
                vm.FormEntry.Course = courseArr[1];
                vm.FormEntry.CourseCRN = courseArr[0];

                // Build the accomodations from the view model
                var accomodations = new List<Accomodation>();

                if (vm.MinimalDistractionsExtendedTime)
                {
                    accomodations.Add(new Accomodation
                    {
                        Type = "general",
                        Value = "Minimal Distractions & Extended Time"
                    });
                }

                if (vm.WordProcessor)
                {
                    accomodations.Add(new Accomodation
                    {
                        Type = "general",
                        Value = "Word Processor"
                    });
                }

                if (vm.TextToSpeech)
                {
                    accomodations.Add(new Accomodation
                    {
                        Type = "general",
                        Value = "Text-to-Speech"
                    });
                }

                if (vm.OtherAccomodations)
                {
                    if (vm.OtherAccomodationsValue != String.Empty)
                    {
                        accomodations.Add(new Accomodation
                        {
                            Type = "general",
                            Value = vm.OtherAccomodationsValue
                        });
                    }
                }

                // If there's a note, create it and add it to the form entry
                if (!String.IsNullOrEmpty(vm.Note))
                {
                    var note = new Note
                    {
                        Type = "student",
                        Value = vm.Note,
                        AddedByPidm = user.BannerIdentity.Pidm,
                        AddedDate = DateTime.Now
                    };

                    vm.FormEntry.Notes.Add(note);
                }

                // Add the accomodations to the form entry
                vm.FormEntry.Accomodations = accomodations;

                // Add a date submitted
                vm.FormEntry.DateSubmitted = DateTime.Now;

                // Save the form entry to the database
                vm.FormEntry = _formEntryService.SaveFormEntry(vm.FormEntry);

                var formEntry = _formEntryService.GetFormEntryById(vm.FormEntry.Id.Value);

                // For whatever reason after saving NHibernate is not pulling the identity
                formEntry.StudentIdentity = _bannerIdentityService.GetBannerIdentityByPidm(formEntry.StudentPidm);
                formEntry.ProfessorIdentity = _bannerIdentityService.GetBannerIdentityByPidm(formEntry.ProfessorPidm);

                // Email the student a receipt
                if (DEBUGMODE)
                    _emailService.SendEmail<FormEntry>(EmailTemplate.StudentReceipt, formEntry, WebConfigurationManager.AppSettings["TestEmailAddress"], "TAS Request - Received");
                else
                    _emailService.SendEmail<FormEntry>(EmailTemplate.StudentReceipt, formEntry, formEntry.StudentIdentity.Email, "TAS Request - Received");
                
                // Email the professor for approval
                if (DEBUGMODE)
                    _emailService.SendEmail<FormEntry>(EmailTemplate.InstructorReceipt, formEntry, WebConfigurationManager.AppSettings["TestEmailAddress"], "Please respond to your student’s request to take an exam at Test Administration Services");
                else
                    _emailService.SendEmail<FormEntry>(EmailTemplate.InstructorReceipt, formEntry, formEntry.ProfessorIdentity.Email, "Please respond to your student’s request to take an exam at Test Administration Services");

                return View("StudentSuccess", formEntry);
            }
            else
            {
                var courses = new List<Course>();

                if (DEBUGMODE)
                    courses = _courseService.GetCoursesByPidm(14277).ToList();
                else
                    courses = _courseService.GetCoursesByPidm(user.BannerIdentity.Pidm).ToList();

                vm.Courses = courses.Select(x =>
                    new SelectListItem
                    {
                        Text = x.Subject + " " + x.Number + " - " + x.Title,
                        Value = x.CRN + "|" + x.Subject + " " + x.Number + " - " + x.Title + "|" + x.ProfessorPidm
                    });

                // Get the latest request to pull in data
                vm.PreviousFormEntry = _formEntryService.GetAllStudentFormEntriesByPidm(user.BannerIdentity.Pidm)
                                            .Where(x => x.ProfessorApprovedDate.HasValue)
                                            .OrderByDescending(x => x.ProfessorApprovedDate)
                                            .FirstOrDefault();

                return View(vm);
            }
        }

        [HttpGet]
        public ActionResult Cancel(int id)
        {
            var user = User as UserPrincipal;

            var testTime = _testTimeService.GetTestTimeById(id);

            if (testTime == null)
                return new HttpNotFoundResult(String.Format("There is no test with id ({0}).", id));

            if (testTime.FormEntry.StudentPidm != user.BannerIdentity.Pidm
                && testTime.FormEntry.ProfessorPidm != user.BannerIdentity.Pidm
                && !user.IsAdmin)
            {
                return new HttpNotFoundResult(String.Format("You do not have permission to cancel test with id ({0}).", id));
            }

            // Update testtime to show canceled
            testTime.Canceled = true;
            testTime.CanceledByPidm = user.BannerIdentity.Pidm;
            testTime.CanceledDate = DateTime.Now;

            // Save to db
            testTime = _testTimeService.SaveTestTime(testTime);

            // If all test times are canceled, then decline the order
            var allTestTimesCanceled = true;
            var formEntry = testTime.FormEntry;

            foreach(var test in formEntry.TestTimes)
            {
                if (!test.Canceled)
                {
                    allTestTimesCanceled = false;
                    break;
                }
            }

            if (allTestTimesCanceled)
            {
                formEntry.Notes.Add(new Note
                {
                    AddedDate = DateTime.Now,
                    Type = "system",
                    AddedByPidm = user.BannerIdentity.Pidm,
                    Value = "This request has been declined due to cancelation of all tests."
                });

                formEntry.TASApproved = false;

                if (!formEntry.ApprovedByPidm.HasValue)
                    formEntry.ApprovedByPidm = user.BannerIdentity.Pidm;

                if (!formEntry.ApprovedDate.HasValue)
                    formEntry.ApprovedDate = DateTime.Now;

                if (!formEntry.ProfessorApprovedDate.HasValue)
                    formEntry.ProfessorApprovedDate = DateTime.Now;

                formEntry = _formEntryService.SaveFormEntry(formEntry);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetCourseMeetingInfo(string id)
        {
            var courseMeetingInfo = _courseMeetingService.GetCourseMeetingInfoByCRN(id);

            return Json(courseMeetingInfo, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetFormEntriesWithConflictingTime(string time)
        {
            var user = User as UserPrincipal;
            var date = DateTime.Parse(time);

            var formEntries = _formEntryService.GetAllFormEntriesWithConflictingTime(date, user.BannerIdentity.Pidm);

            return Json(formEntries.Count > 0, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult CheckCourseTimeMismatch(string courseTime, string examTime)
        {
            var formEntry = new FormEntry {
                CourseTime = courseTime
            };

            var testTime = new TestTime {
                Date = DateTime.Parse(examTime),
                Type = "quiz"
            };

            formEntry.TestTimes.Add(testTime);

            var mismatch = _formEntryService.CourseTimeMismatch(formEntry);

            return Json(mismatch, JsonRequestBehavior.AllowGet);
        }
    }
}