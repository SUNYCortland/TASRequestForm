using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TASRequestForm.Core.Services;
using TASRequestForm.Web.Authentication;

namespace TASRequestForm.Web.Controllers
{
    [Authorize]
    public class StudentController : Controller
    {
        private readonly IFormEntryService _formEntryService;
        private readonly ITestTimeService _testTimeService;

        public StudentController(IFormEntryService formEntryService,
                                    ITestTimeService testTimeService)
        {
            _formEntryService = formEntryService;
            _testTimeService = testTimeService;
        }

        //
        // GET: /Student/Pending
        public ActionResult Pending()
        {
            var user = User as UserPrincipal;

            var tests = _testTimeService.GetAllStudentPendingTestTimes(user.BannerIdentity.Pidm);

            return View(tests);
        }

        //
        // GET: /Student/Approved
        public ActionResult Approved()
        {
            var user = User as UserPrincipal;

            var tests = _testTimeService.GetAllStudentApprovedTestTimes(user.BannerIdentity.Pidm);

            return View(tests);
        }

        //
        // GET: /Student/Declined
        public ActionResult Declined()
        {
            var user = User as UserPrincipal;

            var tests = _testTimeService.GetAllStudentDeclinedTestTimes(user.BannerIdentity.Pidm);

            return View(tests);
        }

        //
        // GET: /Student/Request/{id}
        public ActionResult Request(int id)
        {
            var user = User as UserPrincipal;

            var request = _formEntryService.GetFormEntryById(id);

            if (request == null)
                return new HttpNotFoundResult(String.Format("Id ({0}) is not a valid id for a TAS Request.", id));

            if (request.StudentPidm != user.BannerIdentity.Pidm)
                return new HttpNotFoundResult(String.Format("You do not have permission to view entry ({0}).", id));

            return View(request);
        }
	}
}