using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TASRequestForm.Core.Services;
using TASRequestForm.Web.Areas.Admin.ViewModels;

namespace TASRequestForm.Web.Areas.Admin.Controllers
{
    public class NavController : Controller
    {
        private readonly ITestTimeService _testTimeService;

        public NavController(ITestTimeService testTimeService)
        {
            _testTimeService = testTimeService;
        }

        // GET: Admin/LoadMenu
        public ActionResult LoadMenu(string active)
        {
            var vm = new NavLoadMenuViewModel();

            var pending = _testTimeService.GetAllPendingTestTimes().OrderByDescending(x => x.Date);
            var approved = _testTimeService.GetAllApprovedTestTimes().OrderByDescending(x => x.Date);
            var declined = _testTimeService.GetAllDeclinedTestTimes().OrderByDescending(x => x.Date);

            var pendingCount = pending.Count();
            var approvedCount = approved.Count();
            var declinedCount = declined.Count();

            vm.PendingCount = pendingCount;
            vm.ApprovedCount = approvedCount;
            vm.DeclinedCount = declinedCount;

            vm.Active = active;

            return PartialView("_MainNavigationMenu", vm);
        }

        // POST: Admin/LoadMenu
        [HttpPost]
        [ActionName("LoadMenu")]
        public ActionResult LoadMenu_Post(string active)
        {
            var vm = new NavLoadMenuViewModel();

            var pending = _testTimeService.GetAllPendingTestTimes().OrderByDescending(x => x.Date);
            var approved = _testTimeService.GetAllApprovedTestTimes().OrderByDescending(x => x.Date);
            var declined = _testTimeService.GetAllDeclinedTestTimes().OrderByDescending(x => x.Date);

            var pendingCount = pending.Count();
            var approvedCount = approved.Count();
            var declinedCount = declined.Count();

            vm.PendingCount = pendingCount;
            vm.ApprovedCount = approvedCount;
            vm.DeclinedCount = declinedCount;

            vm.Active = active;

            return PartialView("_MainNavigationMenu", vm);
        }
    }
}