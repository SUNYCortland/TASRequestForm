using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TASRequestForm.Core.Data.Entities;
using TASRequestForm.Core.Services;
using TASRequestForm.Web.Authentication;
using TASRequestForm.Web.ViewModels;
using TASRequestForm.Web.Extensions;
using System.Web.Configuration;

namespace TASRequestForm.Web.Controllers
{
    [Authorize]
    public class InstructorController : Controller
    {
        private bool DEBUGMODE = bool.Parse(WebConfigurationManager.AppSettings["DebugMode"]);

        private readonly IFormEntryService _formEntryService;
        private readonly IBannerIdentityService _bannerIdentityService;
        private readonly IEmailService _emailService;
        private readonly ITestTimeService _testTimeService;

        public InstructorController(IFormEntryService formEntryService,
                                    IBannerIdentityService bannerIdentityService,
                                    IEmailService emailService,
                                    ITestTimeService testTimeService)
        {
            _formEntryService = formEntryService;
            _bannerIdentityService = bannerIdentityService;
            _emailService = emailService;
            _testTimeService = testTimeService;
        }

        //
        // GET: /Instructor/Request/{id}

        public ActionResult Request(int id)
        {
            var vm = new InstructorRequestViewModel();

            vm.FormEntry = _formEntryService.GetFormEntryById(id);

            if (vm.FormEntry == null)
                return HttpNotFound();

            // This entry has already been approved by the professor
            if (vm.FormEntry.ProfessorApprovedDate.HasValue)
                return RedirectToAction("View", new { id = id });

            // Get the latest professor approved request to pull in data
            vm.PreviousFormEntry = _formEntryService.GetAllProfessorFormEntriesByPidm(vm.FormEntry.ProfessorIdentity.Pidm)
                                        .Where(x => x.ProfessorApprovedDate.HasValue)
                                        .OrderByDescending(x => x.ProfessorApprovedDate)
                                        .FirstOrDefault();

            vm.CourseTimeMismatch = _formEntryService.CourseTimeMismatch(vm.FormEntry);

            if (vm.FormEntry.TestTimes.Count > 1)
                vm.MultipleRequests = true;
            else
                vm.MultipleRequests = false;

            return View(vm);
        }

        //
        // POST: /Instructor/Request/{id}

        [HttpPost]
        public ActionResult Request(int id, InstructorRequestViewModel vm)
        {
            var user = User as UserPrincipal;
            var formEntry = _formEntryService.GetFormEntryById(id);

            formEntry.DeliveryType = vm.FormEntry.DeliveryType;
            formEntry.ReturnType = vm.FormEntry.ReturnType;
            formEntry.ProfessorCampusAddress = vm.FormEntry.ProfessorCampusAddress;
            formEntry.ProfessorEmail = vm.FormEntry.ProfessorEmail;
            formEntry.ProfessorReachedBy = vm.FormEntry.ProfessorReachedBy;
            formEntry.ReminderDays = vm.FormEntry.ReminderDays;

            if (ModelState.IsValid)
            {
                formEntry.ProfessorApprovedDate = DateTime.Now;

                // If this request was denied
                if (vm.Denied)
                {
                    formEntry.ProfessorApproved = false;
                    formEntry.ApprovedByPidm = user.BannerIdentity.Pidm;
                    formEntry.ApprovedDate = DateTime.Now;

                    // We will use this note to notify requestor of denial reason
                    var note = new Note
                    {
                        Type = "professor",
                        Value = vm.Note,
                        AddedByPidm = user.BannerIdentity.Pidm,
                        AddedDate = DateTime.Now
                    };

                    formEntry.Notes.Add(note);

                    // Save the form entry
                    formEntry = _formEntryService.SaveFormEntry(formEntry);

                    // For whatever reason after saving NHibernate is not pulling the identity
                    formEntry.StudentIdentity = _bannerIdentityService.GetBannerIdentityByPidm(formEntry.StudentPidm);
                    formEntry.ProfessorIdentity = _bannerIdentityService.GetBannerIdentityByPidm(formEntry.ProfessorPidm);

                    // Notify requestor of denial
                    if (DEBUGMODE)
                        _emailService.SendEmail<FormEntry>(EmailTemplate.InstructorDenied, formEntry, WebConfigurationManager.AppSettings["TestEmailAddress"], "TAS Request - Instructor Denied Request");
                    else
                        _emailService.SendEmail<FormEntry>(EmailTemplate.InstructorDenied, formEntry, formEntry.StudentIdentity.Email, "TAS Request - Instructor Denied Request");

                    return View("InstructorDenied", formEntry);
                }

                /*
                 * If this request was approved
                 */

                formEntry.ProfessorApproved = true;

                // If test time is less than 5 business days from when form was submitted
                // then this form requires TAS approval
                var tasApproval = false;
                foreach (var testTime in formEntry.TestTimes)
                {
                    if (formEntry.DateSubmitted.BusinessDaysTo(testTime.Date) < 5 && !testTime.Canceled)
                    {
                        tasApproval = true;
                    }
                }

                if (!tasApproval)
                {
                    formEntry.TASApproved = true;
                    formEntry.ApprovedDate = DateTime.Now;
                    formEntry.ApprovedByPidm = user.BannerIdentity.Pidm;
                }

                // Book
                if (vm.BookQuizAccomodation)
                    formEntry.Accomodations.Add(new Accomodation
                    {
                        Type = "quiz",
                        Value = "Book"
                    });

                if (vm.BookExamAccomodation)
                    formEntry.Accomodations.Add(new Accomodation
                    {
                        Type = "exam",
                        Value = "Book"
                    });

                if (vm.BookFinalExamAccomodation)
                    formEntry.Accomodations.Add(new Accomodation
                    {
                        Type = "final",
                        Value = "Book"
                    });

                // Notes
                if (vm.NotesQuizAccomodation)
                    formEntry.Accomodations.Add(new Accomodation
                    {
                        Type = "quiz",
                        Value = "Notes"
                    });

                if (vm.NotesExamAccomodation)
                    formEntry.Accomodations.Add(new Accomodation
                    {
                        Type = "exam",
                        Value = "Notes"
                    });

                if (vm.NotesFinalExamAccomodation)
                    formEntry.Accomodations.Add(new Accomodation
                    {
                        Type = "final",
                        Value = "Notes"
                    });

                // Calculator
                if (vm.CalculatorQuizAccomodation)
                    formEntry.Accomodations.Add(new Accomodation
                    {
                        Type = "quiz",
                        Value = "Calculator"
                    });

                if (vm.CalculatorExamAccomodation)
                    formEntry.Accomodations.Add(new Accomodation
                    {
                        Type = "exam",
                        Value = "Calculator"
                    });

                if (vm.CalculatorFinalExamAccomodation)
                    formEntry.Accomodations.Add(new Accomodation
                    {
                        Type = "final",
                        Value = "Calculator"
                    });

                // Formulae
                if (vm.FormulaeQuizAccomodation)
                    formEntry.Accomodations.Add(new Accomodation
                    {
                        Type = "quiz",
                        Value = "Formulae"
                    });

                if (vm.FormulaeExamAccomodation)
                    formEntry.Accomodations.Add(new Accomodation
                    {
                        Type = "exam",
                        Value = "Formulae"
                    });

                if (vm.FormulaeFinalExamAccomodation)
                    formEntry.Accomodations.Add(new Accomodation
                    {
                        Type = "final",
                        Value = "Formulae"
                    });

                // Other
                if (vm.OtherQuizAccomodation)
                    formEntry.Accomodations.Add(new Accomodation
                    {
                        Type = "quiz",
                        Value = vm.OtherQuizAccomodationString
                    });

                if (vm.OtherExamAccomodation)
                    formEntry.Accomodations.Add(new Accomodation
                    {
                        Type = "exam",
                        Value = vm.OtherExamAccomodationString
                    });

                if (vm.OtherFinalExamAccomodation)
                    formEntry.Accomodations.Add(new Accomodation
                    {
                        Type = "final",
                        Value = vm.OtherFinalExamAccomodationString
                    });

                // None
                if (vm.NoQuizAccomodation)
                    formEntry.Accomodations.Add(new Accomodation
                    {
                        Type = "quiz",
                        Value = "None"
                    });

                if (vm.NoExamAccomodation)
                    formEntry.Accomodations.Add(new Accomodation
                    {
                        Type = "exam",
                        Value = "None"
                    });

                if (vm.NoFinalExamAccomodation)
                    formEntry.Accomodations.Add(new Accomodation
                    {
                        Type = "final",
                        Value = "None"
                    });

                // If there's a note, create it and add it to the form entry
                if (!String.IsNullOrEmpty(vm.Note))
                {
                    var note = new Note
                    {
                        Type = "professor",
                        Value = vm.Note,
                        AddedByPidm = user.BannerIdentity.Pidm,
                        AddedDate = DateTime.Now
                    };

                    formEntry.Notes.Add(note);
                }

                // Save the form entry
                formEntry = _formEntryService.SaveFormEntry(formEntry);

                // For whatever reason after saving NHibernate is not pulling the identity
                formEntry.StudentIdentity = _bannerIdentityService.GetBannerIdentityByPidm(formEntry.StudentPidm);
                formEntry.ProfessorIdentity = _bannerIdentityService.GetBannerIdentityByPidm(formEntry.ProfessorPidm);

                // Notify requestor of approval
                if (DEBUGMODE)
                    _emailService.SendEmail<FormEntry>(EmailTemplate.InstructorApproval, formEntry, WebConfigurationManager.AppSettings["TestEmailAddress"], "TAS Request - Instructor Approved Request");
                else
                    _emailService.SendEmail<FormEntry>(EmailTemplate.InstructorApproval, formEntry, formEntry.StudentIdentity.Email, "TAS Request - Instructor Approved Request");

                return View("InstructorSuccess", formEntry);
            }

            // Get the latest professor approved request to pull in data
            vm.PreviousFormEntry = _formEntryService.GetAllProfessorFormEntriesByPidm(formEntry.ProfessorIdentity.Pidm)
                                        .Where(x => x.ProfessorApprovedDate.HasValue)
                                        .OrderByDescending(x => x.ProfessorApprovedDate)
                                        .FirstOrDefault();

            vm.FormEntry = formEntry;
            vm.Denied = false;

            vm.CourseTimeMismatch = _formEntryService.CourseTimeMismatch(vm.FormEntry);

            if (vm.FormEntry.TestTimes.Count > 1)
                vm.MultipleRequests = true;
            else
                vm.MultipleRequests = false;

            return View(vm);
        }

        //
        // GET: /Instructor/Approved
        public ActionResult Approved()
        {
            var user = User as UserPrincipal;

            var tests = _testTimeService.GetAllInstructorApprovedTestTimes(user.BannerIdentity.Pidm);

            return View(tests);
        }

        //
        // GET: /Instructor/Pending
        public ActionResult Pending()
        {
            var user = User as UserPrincipal;

            var tests = _testTimeService.GetAllInstructorPendingTestTimes(user.BannerIdentity.Pidm);

            return View(tests);
        }

        //
        // GET: /Instructor/Declined
        public ActionResult Declined()
        {
            var user = User as UserPrincipal;

            var tests = _testTimeService.GetAllInstructorDeclinedTestTimes(user.BannerIdentity.Pidm);

            return View(tests);
        }

        //
        // GET: /Instructor/View/{id}
        public ActionResult View(int id)
        {
            var formEntry = _formEntryService.GetFormEntryById(id);

            if (formEntry == null)
            {
                return new HttpNotFoundResult(String.Format("Id ({0}) is not a valid id for a TAS Request.", id));
            }

            return View(formEntry);
        }
	}
}