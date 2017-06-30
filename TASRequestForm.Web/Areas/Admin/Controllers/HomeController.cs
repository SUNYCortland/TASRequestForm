using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using TASRequestForm.Core.Services;
using TASRequestForm.Web.Areas.Admin.Attributes;
using TASRequestForm.Web.Areas.Admin.ViewModels;

namespace TASRequestForm.Web.Areas.Admin.Controllers
{
    [AdminAuthorize]
    public class HomeController : Controller
    {
        private bool DEBUGMODE = bool.Parse(WebConfigurationManager.AppSettings["DebugMode"]);

        private readonly IFormEntryService _formEntryService;
        private readonly ITestTimeService _testTimeService;

        public HomeController(IFormEntryService formEntryService, ITestTimeService testTimeService)
        {
            _formEntryService = formEntryService;
            _testTimeService = testTimeService;
        }

        //
        // GET: /Admin/Home/
        public ActionResult Index()
        {
            var vm = new HomeIndexViewModel();

            vm.Pending = _testTimeService.GetAllPendingTestTimes().OrderBy(x => x.Date).Take(5).ToList();
            vm.Approved = _testTimeService.GetAllApprovedTestTimes().Where(x => x.Date >= DateTime.Now).OrderByDescending(x => x.Date).Take(5).ToList();
            vm.Declined = _testTimeService.GetAllDeclinedTestTimes().Where(x => x.Date >= DateTime.Now).OrderByDescending(x => x.Date).Take(5).ToList();

            return View(vm);
        }
	}
}