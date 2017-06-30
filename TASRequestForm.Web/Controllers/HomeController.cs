using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TASRequestForm.Web.Extensions;
using TASRequestForm.Core.Data.Entities;
using TASRequestForm.Core.Services;
using TASRequestForm.Web.Authentication;
using TASRequestForm.Web.ViewModels;

namespace TASRequestForm.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IFormEntryService _formEntryService;
        private readonly ITestTimeService _testTimeService;

        public HomeController(IFormEntryService formEntryService,
                                ITestTimeService testTimeService)
        {
            _formEntryService = formEntryService;
            _testTimeService = testTimeService;
        }

        public ActionResult Index()
        {
            var user = User as UserPrincipal;
            var vm = new HomeIndexViewModel();

            vm.StudentEntries = _testTimeService.GetAllStudentTestTimes(user.BannerIdentity.Pidm).ToList();
            vm.ProfessorEntries = _testTimeService.GetAllInstructorTestTimes(user.BannerIdentity.Pidm).ToList();

            return View(vm);
        }
    }
}