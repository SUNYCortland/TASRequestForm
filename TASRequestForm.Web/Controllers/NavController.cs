using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TASRequestForm.Core.Services;
using TASRequestForm.Web.Authentication;
using TASRequestForm.Web.ViewModels;

namespace TASRequestForm.Web.Controllers
{
    public class NavController : Controller
    {
        private readonly ITestTimeService _testTimeService;

        public NavController(ITestTimeService testTimeService)
        {
            _testTimeService = testTimeService;
        }

        // GET: Nav/LoadMenu
        [HttpGet]
        public ActionResult LoadMenu(string active)
        {
            var user = User as UserPrincipal;
            var vm = new NavLoadMenuViewModel();

            vm.StudentEntries = _testTimeService.GetAllStudentTestTimes(user.BannerIdentity.Pidm).ToList();
            vm.ProfessorEntries = _testTimeService.GetAllInstructorTestTimes(user.BannerIdentity.Pidm).ToList();

            vm.Active = active;

            return PartialView("_MainNavigationMenu", vm);
        }

        // POST: Nav/LoadMenu
        [HttpPost]
        [ActionName("LoadMenu")]
        public ActionResult LoadMenu_Post(string active)
        {
            var user = User as UserPrincipal;
            var vm = new NavLoadMenuViewModel();

            vm.StudentEntries = _testTimeService.GetAllStudentTestTimes(user.BannerIdentity.Pidm).ToList();
            vm.ProfessorEntries = _testTimeService.GetAllInstructorTestTimes(user.BannerIdentity.Pidm).ToList();

            vm.Active = active;

            return PartialView("_MainNavigationMenu", vm);
        }
    }
}