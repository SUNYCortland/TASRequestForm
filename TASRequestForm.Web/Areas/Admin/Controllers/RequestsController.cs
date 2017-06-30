using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using TASRequestForm.Core.Data.Entities;
using TASRequestForm.Core.Services;
using TASRequestForm.Web.Areas.Admin.Attributes;
using TASRequestForm.Web.Areas.Admin.ViewModels;
using TASRequestForm.Web.Authentication;
using TASRequestForm.Web.Extensions;
using TASRequestForm.Common.Pagination;

namespace TASRequestForm.Web.Areas.Admin.Controllers
{
    [AdminAuthorize]
    public class RequestsController : Controller
    {
        private bool DEBUGMODE = bool.Parse(WebConfigurationManager.AppSettings["DebugMode"]);
        private const int DEFAULT_PAGE_SIZE = 25;

        private readonly IFormEntryService _formEntryService;
        private readonly IBannerIdentityService _bannerIdentityService;
        private readonly IEmailService _emailService;
        private readonly ITestTimeService _testTimeService;
        private readonly ICourseService _courseService;

        public RequestsController(IFormEntryService formEntryService,
                                    IBannerIdentityService bannerIdentityService,
                                    IEmailService emailService,
                                    ITestTimeService testTimeService,
                                    ICourseService courseService)
        {
            _formEntryService = formEntryService;
            _bannerIdentityService = bannerIdentityService;
            _emailService = emailService;
            _testTimeService = testTimeService;
            _courseService = courseService;
        }

        //
        // GET: /Admin/Requests/View/{id}
        public ActionResult View(int id)
        {
            var formEntry = _formEntryService.GetFormEntryById(id);

            if (formEntry == null)
            {
                return new HttpNotFoundResult(String.Format("Id ({0}) is not a valid id for a TAS Request.", id));
            }

            return View(formEntry);
        }

        //
        // GET: /Admin/Requests/Pending
        public ActionResult Pending(int? page)
        {
            int currentPageIndex = page.HasValue ? page.Value - 1 : 0;
            var testTimes = _testTimeService.GetAllPendingTestTimes();

            var testTimesPaged = testTimes.ToPagedList(currentPageIndex, DEFAULT_PAGE_SIZE);

            if (currentPageIndex >= testTimesPaged.PageCount)
                return View(testTimes.ToPagedList(0, DEFAULT_PAGE_SIZE));

            return View(testTimesPaged);
        }

        //
        // GET: /Admin/Requests/Declined
        public ActionResult Declined(int? page)
        {
            int currentPageIndex = page.HasValue ? page.Value - 1 : 0;
            var testTimes = _testTimeService.GetAllDeclinedTestTimes();

            var testTimesPaged = testTimes.ToPagedList(currentPageIndex, DEFAULT_PAGE_SIZE);

            if (currentPageIndex >= testTimesPaged.PageCount)
                return View(testTimes.ToPagedList(0, DEFAULT_PAGE_SIZE));

            return View(testTimesPaged);
        }

        //
        // GET: /Admin/Requests/Approved
        public ActionResult Approved(int? page, string filter)
        {
            int currentPageIndex = page.HasValue ? page.Value - 1 : 0;
            var testTimes = _testTimeService.GetAllApprovedTestTimes();

            if (String.IsNullOrEmpty(filter))
                filter = "future";

            if (!String.IsNullOrEmpty(filter))
            {
                filter = filter.ToUpper();

                switch (filter)
                {
                    case "PAST":
                        testTimes = testTimes.Where(x => x.Date <= DateTime.Now);
                        break;

                    case "FUTURE":
                        testTimes = testTimes.Where(x => x.Date >= DateTime.Now);
                        break;

                    case "ALL":
                        break;

                    default:
                        break;
                }
            }

            var testTimesPaged = testTimes.ToPagedList(currentPageIndex, DEFAULT_PAGE_SIZE);

            if (currentPageIndex >= testTimesPaged.PageCount)
                return View(testTimes.ToPagedList(0, DEFAULT_PAGE_SIZE));

            return View(testTimesPaged);
        }

        //
        // GET: /Admin/Requests/TASApprove/{id}
        public ActionResult TASApprove(int id)
        {
            var formEntry = _formEntryService.GetFormEntryById(id);

            if (formEntry == null)
            {
                return new HttpNotFoundResult(String.Format("Id ({0}) is not a valid id for a TAS Request.", id));
            }

            if (!formEntry.ProfessorApprovedDate.HasValue)
            {
                return new HttpNotFoundResult(String.Format("Form entry ({0}) is valid but has not yet been approved by the instructur.", id));
            }

            if (formEntry.ApprovedDate.HasValue)
            {
                return new HttpNotFoundResult(String.Format("Form entry ({0}) is valid but has already been approved.", id));
            }

            var vm = new RequestsTasApproveViewModel();

            vm.FormEntry = formEntry;

            return View(vm);
        }

        //
        // POST: /Admin/Requests/TASApprove/{id}
        [HttpPost]
        public ActionResult TASApprove(int id, RequestsTasApproveViewModel vm)
        {
            var user = User as UserPrincipal;
            var formEntry = _formEntryService.GetFormEntryById(id);

            if (formEntry == null)
            {
                return new HttpNotFoundResult(String.Format("Id ({0}) is not a valid id for a TAS Request.", id));
            }

            if (!formEntry.ProfessorApprovedDate.HasValue)
            {
                return new HttpNotFoundResult(String.Format("Form entry ({0}) is valid but has not yet been approved by the instructur.", id));
            }

            if (formEntry.ApprovedDate.HasValue)
            {
                return new HttpNotFoundResult(String.Format("Form entry ({0}) is valid but has already been approved.", id));
            }

            if (ModelState.IsValid)
            {
                formEntry.ApprovedDate = DateTime.Now;
                formEntry.ApprovedByPidm = user.BannerIdentity.Pidm;

                // If this request was denied by TAS
                if (vm.Denied)
                {
                    formEntry.TASApproved = false;

                    // We will use this note to notify requestor of denial reason
                    var note = new Note
                    {
                        Type = "tas",
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
                        _emailService.SendEmail<FormEntry>(EmailTemplate.TASDenied, formEntry, WebConfigurationManager.AppSettings["TestEmailAddress"], "TAS Request - TAS Denied Request");
                    else
                        _emailService.SendEmail<FormEntry>(EmailTemplate.TASDenied, formEntry, formEntry.StudentIdentity.Email, "TAS Request - TAS Denied Request");

                    return View("TASDenied", formEntry);
                }

                /*
                 * TAS has approved this request
                 */ 

                formEntry.TASApproved = true;

                // If there's a note, create it and add it to the form entry
                if (!String.IsNullOrEmpty(vm.Note))
                {
                    var note = new Note
                    {
                        Type = "tas",
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
                    _emailService.SendEmail<FormEntry>(EmailTemplate.TASApproval, formEntry, WebConfigurationManager.AppSettings["TestEmailAddress"], "TAS Request - TAS Approved Request");
                else
                    _emailService.SendEmail<FormEntry>(EmailTemplate.TASApproval, formEntry, formEntry.StudentIdentity.Email, "TAS Request - TAS Approved Request");

                return View("TASSuccess", formEntry);
            }

            vm.FormEntry = formEntry;
            vm.Denied = false;

            return View(vm);
        }

        //
        // GET: /Admin/Requests/InstructorApprove/{id}
        public ActionResult InstructorApprove(int id)
        {
            var formEntry = _formEntryService.GetFormEntryById(id);

            if (formEntry == null)
            {
                return new HttpNotFoundResult(String.Format("Id ({0}) is not a valid id for a TAS Request.", id));
            }

            if (formEntry.ProfessorApprovedDate.HasValue)
            {
                return new HttpNotFoundResult(String.Format("Form entry ({0}) has already been approved by the instructor.", id));
            }

            if (formEntry.ApprovedDate.HasValue)
            {
                return new HttpNotFoundResult(String.Format("Form entry ({0}) is valid but has already been approved.", id));
            }

            var vm = new RequestsInstructorApproveViewModel();

            vm.FormEntry = formEntry;

            return View(vm);
        }

        //
        // POST: /Admin/Requests/InstructorApprove/{id}
        [HttpPost]
        public ActionResult InstructorApprove(int id, RequestsInstructorApproveViewModel vm)
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
                        Type = "tas",
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
                        Type = "tas",
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

            vm.FormEntry = formEntry;
            vm.Denied = false;

            return View(vm);
        }

        //
        // GET: /Admin/Requests/Submit
        public ActionResult Submit()
        {
            var vm = new RequestsSubmitViewModel();

            return View(vm);
        }

        //
        // POST: /Admin/Requests/Submit
        [HttpPost]
        public ActionResult Submit(RequestsSubmitViewModel vm)
        {
            var user = User as UserPrincipal;

            if (ModelState.IsValid)
            {
                var courseArr = vm.SelectedCourse.Split('|');

                // Add student pidm
                vm.FormEntry.StudentPidm = vm.StudentPidm;

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
                // Check if email has been bypassed
                if (vm.BypassProfessorEmail)
                {
                    if (!String.IsNullOrEmpty(vm.ProfessorEmailText))
                    {
                        // Add text as a note to form entry
                        var note = new Note
                        {
                            Type = "tas",
                            Value = vm.ProfessorEmailText,
                            AddedByPidm = user.BannerIdentity.Pidm,
                            AddedDate = DateTime.Now
                        };

                        formEntry.Notes.Add(note);

                        // Send email
                        if (DEBUGMODE)
                            _emailService.SendEmail<FormEntry>(EmailTemplate.InstructorBypass, formEntry, WebConfigurationManager.AppSettings["TestEmailAddress"], "TAS Request - Requires Approval");
                        else
                            _emailService.SendEmail<FormEntry>(EmailTemplate.InstructorBypass, formEntry, formEntry.ProfessorIdentity.Email, "TAS Request - Requires Approval");
                    }
                }
                else
                {
                    if (DEBUGMODE)
                        _emailService.SendEmail<FormEntry>(EmailTemplate.InstructorReceipt, formEntry, WebConfigurationManager.AppSettings["TestEmailAddress"], "Please respond to your student’s request to take an exam at Test Administration Services");
                    else
                        _emailService.SendEmail<FormEntry>(EmailTemplate.InstructorReceipt, formEntry, formEntry.ProfessorIdentity.Email, "Please respond to your student’s request to take an exam at Test Administration Services");
                }
                
                return View("SubmitSuccess", formEntry);
            }
            else
            {
                var courses = _courseService.GetCoursesByPidm(vm.StudentPidm);

                vm.Courses = courses.Select(x =>
                    new SelectListItem
                    {
                        Text = x.Subject + " " + x.Number + " - " + x.Title,
                        Value = x.CRN + "|" + x.Subject + " " + x.Number + " - " + x.Title + "|" + x.ProfessorPidm
                    });

                return View(vm);
            }
        }

        //
        // GET: /Admin/Requests/GetStudentData?cnumber={cnumber}
        public ActionResult GetStudentData(string cnumber)
        {
            var studentIdentity = _bannerIdentityService.GetBannerIdentityByCNumber(cnumber);

            if (studentIdentity == null)
                return new HttpNotFoundResult(String.Format("No student was found with C# ({0}).", cnumber));

            var courses = _courseService.GetCoursesByPidm(studentIdentity.Pidm).ToList();

            var vm = new RequestsGetStudentData();

            vm.Student = studentIdentity;
            vm.Courses = courses;

            return Json(vm, JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /Admin/Requests/AddPrivateNote/{id}
        public ActionResult AddPrivateNote(int id, RequestsAddPrivateNoteViewModel vm)
        {
            var user = User as UserPrincipal;

            var formEntry = _formEntryService.GetFormEntryById(id);

            if (formEntry == null)
                return new HttpNotFoundResult(String.Format("No form entry with id ({0}) found.", id));

            if (!String.IsNullOrEmpty(vm.Note))
            {
                var note = new Note
                {
                    Type = "private",
                    Value = vm.Note,
                    AddedByPidm = user.BannerIdentity.Pidm,
                    AddedByIdentity = user.BannerIdentity,
                    AddedDate = DateTime.Now
                };

                formEntry.Notes.Add(note);

                formEntry = _formEntryService.SaveFormEntry(formEntry);
            }

            return PartialView("_PrivateNotes", formEntry.PrivateNotes);
        }

        //
        // GET: /Admin/Requests/NoShowTest/{id}
        public ActionResult NoShowTest(int id)
        {
            var testTime = _testTimeService.GetTestTimeById(id);

            testTime.NoShow = !testTime.NoShow;

            testTime = _testTimeService.SaveTestTime(testTime);

            return RedirectToAction("View", new { id = testTime.FormEntry.Id });
        }
	}
}